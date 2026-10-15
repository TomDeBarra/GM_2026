using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AdaptivePerformance;
using UnityEngine.UI;

public class SpherePhysics : MonoBehaviour, ICollidable
{
    float d0;
    float d0sphere;
    Vector3 oldVelocity;
    Vector3 oldPosition;
    Vector3 velocity = Vector3.zero;
    Vector3 acceleration = Vector3.zero;
    public float mass = 1f;
    public float getCoR() { return 0.75f; }
    public float Radius 
       { get { return transform.localScale.x / 2f; }
        set { transform.localScale = Vector3.one * value;}
    }

    public Vector3 getVelocity() { return this.velocity; }
    public Vector3 getAcceleration() { return this.acceleration; }
    public float getd0() { return this.d0; }
    public Vector3 getOldVelocity() { return this.oldVelocity; }
    public Vector3 getOldPosition() { return this.oldPosition; }
    public void setVelocity(Vector3 velocity) { this.velocity = velocity; }
    public void setd0(float d1) { this.d0 = d1; }

    bool collidingWith(ICollidable c)
    {
        if (c is PlanePhysics plane)
        {
            float d1 = parallel_Distance(transform.position - plane.transform.position, plane.Normal) - Radius;
            if (d1 < 0) return true;
        }
        if (c is SpherePhysics sphere)
        {
            float sumOfRadii = Radius + sphere.Radius;
            float d1 = Vector3.Distance(transform.position, sphere.transform.position);
            if (d1 < sumOfRadii) return true; 
        }
        return false;
    }


    bool ICollidable.collidingWith(ICollidable c)
    {
        return collidingWith(c);
    }

    void Start()
    {
        print(transform.position);
    }

    void Update()
    {
        acceleration = new Vector3(0, -9.81f, 0);

        oldVelocity = velocity;
        oldPosition = transform.position;

        velocity += acceleration * Time.deltaTime;

        transform.position += velocity * Time.deltaTime;

    }

    float parallel_Distance(Vector3 v, Vector3 n)
    {
        return Vector3.Dot(v, n.normalized);
    }
    Vector3 parallel_component(Vector3 v, Vector3 n)
    {
        return (Vector3.Dot(v, n.normalized) * n.normalized);
    }
    Vector3 perp_component(Vector3 v, Vector3 n)
    {
        return v - parallel_component(v, n);
    }

    public void resolve(ICollidable c)
    {
      if (c is PlanePhysics)
        {
            c.resolve(this);
        }

      else if(c is SpherePhysics otherSphere)
        {
            float sumOfRadii = Radius + otherSphere.Radius;
            float d1 = Vector3.Distance(transform.position, otherSphere.transform.position) - sumOfRadii;

            float distanceAtToI = d1 - d0sphere;
            float vMovement = d1 - d0sphere / Time.deltaTime;
            float tImpact = -d0sphere / vMovement;

            Vector3 vImpact = oldVelocity + acceleration * tImpact;
            Vector3 pImpact = oldPosition + vImpact * tImpact;

            Vector3 collisionNormal = (oldPosition - otherSphere.transform.position).normalized;

            Vector3 v1perp = perp_component(vImpact, collisionNormal);
            Vector3 v2perp = perp_component(otherSphere.velocity, collisionNormal);

            float m1 = mass;
            float m2 = otherSphere.mass;

            float resultantParallelVelocity1 = m1 - m2 / (m1 + m2) + (m2 * 2) / (m1 + m2);
            float resultantParallelVelocity2 = m2 - m1 / (m1 + m2) + (m1 * 2) / (m1 + m2);
            Vector3 resultantVelocity1 = getCoR() * (resultantParallelVelocity1 * collisionNormal) + v1perp;
            Vector3 resultantVelocity2 = getCoR() * (resultantParallelVelocity2 * -collisionNormal) + v2perp;

            float tRemaining = Time.deltaTime - tImpact;

            Vector3 finalVelocity1 = resultantVelocity1 + acceleration * tRemaining;
            Vector3 finalVelocity2 = resultantVelocity1 + acceleration * tRemaining;

            // Position is only calculated for this sphere
            Vector3 finalPosition1 = pImpact + finalVelocity1 * tRemaining;

            this.velocity = resultantVelocity1;
            otherSphere.velocity = resultantVelocity2;

            this.transform.position = finalPosition1;

            d0sphere = d1;
        }
    }
}
