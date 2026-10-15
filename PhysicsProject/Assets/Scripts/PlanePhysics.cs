using UnityEngine;

public class PlanePhysics : MonoBehaviour, ICollidable
{
    public Vector3 Normal
    { 
        get { return transform.up; }
        set { transform.up = value; }
    }

    public bool IsCollidingWithSphere { get; private set; }

    bool collidingWith(ICollidable c)
    {
        if (c is PlanePhysics)
            return false;
        SpherePhysics sphere = c as SpherePhysics;
        return PhysicsManager.parallel_Distance(sphere.transform.position - transform.position, Normal) - sphere.Radius < 0;
    }

    bool ICollidable.collidingWith(ICollidable c)
    {
        return collidingWith(c);
    }

    public void resolve(ICollidable c)
    {
        if (c is SpherePhysics sphere)
        {
            float d1 = PhysicsManager.parallel_Distance(sphere.transform.position - transform.position, Normal) - sphere.Radius;
            if (d1 < 0)
            {
                IsCollidingWithSphere = true;

                Vector3 velocity = sphere.getVelocity();

                Vector3 parallelComponent = PhysicsManager.parallel_component(velocity, Normal);
                Vector3 perpendicularComponent = PhysicsManager.perp_component(velocity, Normal);


                float vDrop = d1 - sphere.getd0() / Time.deltaTime;

                float tImpact = -sphere.getd0() / vDrop;

                Vector3 vImpact = sphere.getOldVelocity() + sphere.getAcceleration() * tImpact;
                Vector3 pImpact = sphere.getOldPosition() + vImpact * tImpact;
                Vector3 vImpactOut = PhysicsManager.perp_component(vImpact, Normal) - (sphere.getCoR() * PhysicsManager.parallel_component(vImpact, Normal));

                float tRemaining = Time.deltaTime - tImpact;

                Vector3 v = vImpactOut + sphere.getAcceleration() * tRemaining;
                Vector3 position = pImpact + v * tRemaining;

                sphere.setVelocity(v);
                sphere.transform.position = position;
            }
            sphere.setd0(d1);
        }
    }
}
