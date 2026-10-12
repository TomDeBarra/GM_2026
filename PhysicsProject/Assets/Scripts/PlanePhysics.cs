using UnityEngine;

public class PlanePhysics : MonoBehaviour, ICollidable
{
    public Vector3 Normal
    { 
        get { return transform.up; }
        set { transform.up = value; }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    bool collidingWith(ICollidable c)
    {
        // Simply detect IF it is a plane or sphere and IF it is colliding, physics calculations will be handled in PhysicsManagers this is purely detection
        // Check if 'c' is an instance of PlanePhysics or SpherePhysics
        if (c is PlanePhysics)
            return false;
        SpherePhysics sphere = c as SpherePhysics;
        return PhysicsManager.parallel_Distance(sphere.transform.position - transform.position, Normal) - sphere.Radius < 0;
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    bool ICollidable.collidingWith(ICollidable c)
    {
        return collidingWith(c);
    }

    public void resolve(ICollidable c/*, ref Vector3 newPosition, ref Vector3 newVelocity*/)
    {
        if (c is SpherePhysics sphere)
        {
            float d1 = PhysicsManager.parallel_Distance(sphere.transform.position - transform.position, Normal) - sphere.Radius;
            if (d1 < 0)
            {
                Vector3 velocity = sphere.getVelocity();

                Vector3 parallelComponent = PhysicsManager.parallel_component(velocity, Normal);
                Vector3 perpendicularComponent = PhysicsManager.perp_component(velocity, Normal);


                float vDrop = d1 - sphere.getd0() / Time.deltaTime; //removed parenthesis around d1 or d0

                float tImpact = -sphere.getd0() / vDrop;

                Vector3 vImpact = sphere.getOldVelocity() + sphere.getAcceleration() * tImpact;
                Vector3 pImpact = sphere.getOldPosition() + vImpact * tImpact;
                Vector3 vImpactOut = PhysicsManager.perp_component(vImpact, Normal) - (sphere.getCoR() * PhysicsManager.parallel_component(vImpact, Normal));

                float tRemaining = Time.deltaTime - tImpact;

                Vector3 v = vImpactOut + sphere.getAcceleration() * tRemaining; // v is velocity
                Vector3 position = pImpact + v * tRemaining;

                sphere.setVelocity(v);
                sphere.transform.position = position;
            }
            sphere.setd0(d1);
        }
    }
}
