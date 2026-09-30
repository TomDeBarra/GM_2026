using System.Linq;
using UnityEngine;
using UnityEngine.AdaptivePerformance;
using UnityEngine.UI;

public class SpherePhysics : MonoBehaviour
{
    public PlanePhysics thePlane;
    public SpherePhysics theSphere;

    float d0;
    float d0sphere;

    public float Radius 
       { get { return transform.localScale.x / 2f; }
        set { transform.localScale = Vector3.one * value;}
    }

    Vector3 velocity = Vector3.zero;
    Vector3 acceleration = Vector3.zero;
    float mass = 1;
    float CoR = 0.75f;

    void Start()
    {
        print(transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        acceleration = new Vector3(0, -9.81f, 0);
        // v = u + a * t
        // velocity = velocity + acceleration * Time.deltaTime; 

        Vector3 oldVelocity = velocity;
        Vector3 oldPosition = transform.position;

        velocity += acceleration * Time.deltaTime;

        // s = u * t

        transform.position += velocity * Time.deltaTime;

        plane_Collisions(oldVelocity, oldPosition);
        // colliding with sphere here
        sphere_Collisions();
    }

    void plane_Collisions(Vector3 oldVelocity, Vector3 oldPosition)
    {
        float d1 = parallel_Distance(transform.position - thePlane.transform.position, thePlane.Normal) - Radius;

        if (d1 < 0)
        {
            Vector3 parallelComponent = parallel_component(velocity, thePlane.Normal);
            Vector3 perpendicularComponent = perp_component(velocity, thePlane.Normal);

            float vDrop = d1 - d0 / Time.deltaTime;
            float tImpact = -d0 / vDrop;

            Vector3 vImpact = oldVelocity + acceleration * tImpact;
            Vector3 pImpact = oldPosition + vImpact * tImpact;
            Vector3 vImpactOut = perp_component(vImpact, thePlane.Normal) - (CoR * parallel_component(vImpact, thePlane.Normal));

            float tRemaining = Time.deltaTime - tImpact;

            Vector3 v = vImpactOut + acceleration * tRemaining; // v is velocity
            Vector3 position = pImpact + v * tRemaining;

            velocity = v;
            transform.position = position;
        }
        d0 = d1;
    }

    void sphere_Collisions()
    {
        // Each sphere has a position, velocity, mass and radius
        // SPH 1 = P0, V0, M1, R1
        // SPH 2 = P1, V1, M2, R1
        // Collision occurs when D < Sum of the two R

        float sumOfRadii = Radius + theSphere.Radius;
        float distance = Vector3.Distance(transform.position, theSphere.transform.position);

        Vector3 collisionNormal = (transform.position - theSphere.transform.position).normalized;

        if (distance < sumOfRadii)
        {
            // v1 = v1 parallel component + v1 perpendicular component (v = velocity)
            // 1 = our sphere, 2 = other sphere

            //Vector3 v1para = parallel_component(velocity, collisionNormal);
            //Vector3 v2para = parallel_component(theSphere.velocity, collisionNormal);

            Vector3 v1perp = perp_component(velocity, collisionNormal);
            Vector3 v2perp = perp_component(theSphere.velocity, collisionNormal);

            float m1 = mass;
            float m2 = theSphere.mass;

            // VA2 = m1 - m2 / m1 + m2 (VA1) + (m2 * 2) / (m1 + m2) (VB1)
            float resultantParallelVelocity1 = m1 - m2 / (m1 + m2) + (m2 * 2) / (m1 + m2);
            float resultantParallelVelocity2 = m2 - m1 / (m1 + m2) + (m1 * 2) / (m1 + m2);
            // I can only presume based on the md that va2 and vb2 are the resultant parallel velocities (v*1||, v*2||)
            // overall velocities formula, v* = CoR * resultantParallelVelocity1 + v1perp
            Vector3 resultantVelocity1 = CoR * (resultantParallelVelocity1 * collisionNormal) + v1perp;
            Vector3 resultantVelocity2 = CoR * (resultantParallelVelocity2 * -collisionNormal) + v2perp; // Minus for the moment as both are executing at the same time causing bugs maybe

            this.velocity = resultantVelocity1;
            theSphere.velocity = resultantVelocity2;
        }

        // float distance = Vector3.Distance (object1.transform.position, object2.transform.position); Taken from online
    }

    void sphere_Collisions_withToI(Vector3 oldVelocity, Vector3 oldPosition)
    {
        float sumOfRadii = Radius + theSphere.Radius;
        float d1 = Vector3.Distance(transform.position, theSphere.transform.position);
        Vector3 collisionNormal = (transform.position - theSphere.transform.position).normalized;

        if (d1 < 0)
        {
            Vector3 v1perp = perp_component(velocity, collisionNormal);
            Vector3 v2perp = perp_component(theSphere.velocity, collisionNormal);

            float m1 = mass;
            float m2 = theSphere.mass;

            float resultantParallelVelocity1 = m1 - m2 / (m1 + m2) + (m2 * 2) / (m1 + m2);
            float resultantParallelVelocity2 = m2 - m1 / (m1 + m2) + (m1 * 2) / (m1 + m2);

            Vector3 resultantVelocity1 = CoR * (resultantParallelVelocity1 * collisionNormal) + v1perp;
            Vector3 resultantVelocity2 = CoR * (resultantParallelVelocity2 * -collisionNormal) + v2perp;

            this.velocity = resultantVelocity1;
            theSphere.velocity = resultantVelocity2;

            // Time of Impact code from plane/sphere collision

            //float vDrop = d1 - d0 / Time.deltaTime;
            //float tImpact = -d0 / vDrop;
            //Vector3 vImpact = oldVelocity + acceleration * tImpact;
            //Vector3 pImpact = oldPosition + vImpact * tImpact;
            //Vector3 vImpactOut = perp_component(vImpact, thePlane.Normal) - (CoR * parallel_component(vImpact, thePlane.Normal));
            //float tRemaining = Time.deltaTime - tImpact;
            //Vector3 v = vImpactOut + acceleration * tRemaining;
            //Vector3 position = pImpact + v * tRemaining;
        }
        d0sphere = d1;
        // float distance = Vector3.Distance (object1.transform.position, object2.transform.position); Taken from online
    }
    // Returns the magnitude of the parallel component of vector v parallel  to n
    // v = Vector to be decomposed
    // n = Unit vector parallel to above component
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
}
