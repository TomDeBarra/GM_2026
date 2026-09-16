using System.Linq;
using UnityEngine;

public class SpherePhysics : MonoBehaviour
{
    public PlanePhysics thePlane;
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

        velocity += acceleration * Time.deltaTime;

        // s = u * t

        transform.position += velocity * Time.deltaTime;

        // transform.position += perp_component(velocity, thePlane.Normal) * Time.deltaTime;

        if (parallel_Distance(transform.position - thePlane.transform.position,
                             thePlane.Normal) < Radius) // detect collision
        {
            Vector3 parallelComponent = parallel_component(velocity, thePlane.Normal);
            Vector3 perpendicularComponent = perp_component(velocity, thePlane.Normal);
            velocity = perpendicularComponent - (parallelComponent * CoR);
            transform.position -= parallelComponent * Time.deltaTime;
        }
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
