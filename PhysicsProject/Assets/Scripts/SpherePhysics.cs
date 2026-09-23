using System.Linq;
using UnityEngine;
using UnityEngine.AdaptivePerformance;
using UnityEngine.UI;

public class SpherePhysics : MonoBehaviour
{
    public PlanePhysics thePlane;
    float d0;
    public float Radius 
       { get { return transform.localScale.x / 2f; }
        set { transform.localScale = Vector3.one * value;}
    }

    Vector3 velocity = Vector3.zero;
    Vector3 acceleration = Vector3.zero;
    float CoR = 0.75f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
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

        float d1 = parallel_Distance(transform.position - thePlane.transform.position, thePlane.Normal) - Radius;
        d0 = d1;

        if (d1 < 0) // detect collision
        {
            Vector3 parallelComponent = parallel_component(velocity, thePlane.Normal);
            Vector3 perpendicularComponent = perp_component(velocity, thePlane.Normal);
            //velocity = perpendicularComponent - (parallelComponent * CoR);
            //transform.position -= parallelComponent * Time.deltaTime;

            // V drop = d1 - d0 divided by time.deltatime

            float vDrop = d1 - d0 / Time.deltaTime;
            // T impact = -d0 divided by drop
            float tImpact = -d0 / vDrop;
            // V impact = old velocity + acceleration * T impact
            // P impact = P0 + V impact + T impact
            Vector3 vImpact = oldVelocity + acceleration * tImpact;

            Vector3 pImpact = oldPosition + vImpact * tImpact;

            // Resolve collision
            // vImpactOut = v perp - CoR * v parallel
            Vector3 vImpactOut = perp_component(velocity, thePlane.Normal) - (CoR * parallel_component(velocity, thePlane.Normal));

            // Fast Forward to current frame 
            // T remaining = T - tImpact
            float tRemaining = Time.deltaTime - tImpact;

            // v = vImpactOut + acceleration * tRemaining (v = vector/velocity?)
            Vector3 v = vImpactOut + acceleration * tRemaining;
            // P = pImpact + velocity * tRemaining
            Vector3 position = pImpact + v * tRemaining;

            velocity = v;
            transform.position = position;
        }

        //d0 = d1;

        //// V drop = d1 - d0 divided by time.deltatime

        //float vDrop = d1 - d0 / Time.deltaTime;
        //// T impact = -d0 divided by drop
        //float tImpact = -d0 / vDrop;
        //// V impact = old velocity + acceleration * T impact
        //// P impact = P0 + V impact + T impact
        //Vector3 vImpact = oldVelocity + acceleration * tImpact;

        //Vector3 pImpact = oldPosition + vImpact * tImpact;

        //// Resolve collision
        //// vImpactOut = v perp - CoR * v parallel
        //Vector3 vImpactOut = perp_component(velocity, thePlane.Normal) - (CoR * parallel_component(velocity, thePlane.Normal));

        //// Fast Forward to current frame 
        //// T remaining = T - tImpact
        //float tRemaining = Time.deltaTime - tImpact;

        //// v = vImpactOut + acceleration * tRemaining (v = vector/velocity?)
        //Vector3 v = vImpactOut + acceleration * tRemaining;
        //// P = pImpact + velocity * tRemaining
        //Vector3 position = pImpact + v * tRemaining;
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
