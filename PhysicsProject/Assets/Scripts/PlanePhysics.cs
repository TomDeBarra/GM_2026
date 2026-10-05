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

    bool CollidingWith(ICollidable c)
    {
        // Simply detect IF it is a plane or sphere and IF it is colliding, physics calculations will be handled in PhysicsManagers this is purely detection
        // Check if 'c' is an instance of PlanePhysics or SpherePhysics
        if (c is PlanePhysics)
        {
            //float d1 = parallel_Distance(transform.position - thePlane.transform.position, thePlane.Normal) - Radius;

            //if (d1 < 0) {return true;}
        }
        if (c is SpherePhysics)
        {

        }
        return false;
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

    // Update is called once per frame
    void Update()
    {
        
    }

    bool ICollidable.CollidingWith(ICollidable c)
    {
        return CollidingWith(c);
    }
}
