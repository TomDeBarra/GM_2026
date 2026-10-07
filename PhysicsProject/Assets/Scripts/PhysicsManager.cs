using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// PhysicsManager holds a fixed list
// Perhaps create a game of just 10 spheres
public class PhysicsManager : MonoBehaviour
{
    List<ICollidable> allCollidables;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
         allCollidables = 
              FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
                                       .OfType<ICollidable>()
                                       .ToList();

        print(allCollidables.Count);
    }

    // Update is called once per frame
    void Update()
    {
        // all physics objects are implementations of the ICollidable interface which has the collidingWith() method
        // call collidingWith() every single frame, if(theSphere.collidingWith(theOtherSphere)
        // for i in array etc....
        // for j in array
        // if i is colliding with j...
        // Then physics manager, NOT spheres will handle collisions (it has all information it needs here)
        // E.g. it will call sphere collisions

        for (int i = 0; i < allCollidables.Count; i++)
        {
            for (int j = 0; j < allCollidables.Count; j++)
            {
                if (allCollidables[i].collidingWith(allCollidables[j]))
                {
                    Vector3 newPosition = Vector3.zero, newVelocity = Vector3.zero;
                    allCollidables[i].resolve(allCollidables[j], ref newPosition, ref newVelocity);
                    //if (allCollidables[i] is SpherePhysics && allCollidables[j] is SpherePhysics)
                    //{
                    //    // sphere_Collisions();
                    //}
                    //if (allCollidables[i] is SpherePhysics sphere1 && allCollidables[j] is PlanePhysics plane1) 
                    //{
                    //    plane_Collisions(plane1, sphere1);
                    //    print("We reached plane collisions");
                    //}
                    //if (allCollidables[i] is PlanePhysics plane2 && allCollidables[j] is SpherePhysics sphere2)
                    //{
                    //    plane_Collisions(plane2, sphere2);
                    //    print("We reached plane collisions");
                    //}
                }
            }
        }
    }

    // This is taken from 
    void plane_Collisions(PlanePhysics thePlane, SpherePhysics theSphere)
    {
        float d1 = parallel_Distance(transform.position - thePlane.transform.position, thePlane.Normal) - theSphere.Radius;

        if (d1 < 0)
        {
            Vector3 parallelComponent = parallel_component(theSphere.getVelocity(), thePlane.Normal);
            Vector3 perpendicularComponent = perp_component(theSphere.getVelocity(), thePlane.Normal);

            float vDrop = d1 - theSphere.getd0() / Time.deltaTime;
            float tImpact = -theSphere.getd0() / vDrop;

            Vector3 vImpact = theSphere.getOldVelocity() + theSphere.getAcceleration() * tImpact;
            Vector3 pImpact = theSphere.getOldPosition() + vImpact * tImpact;
            Vector3 vImpactOut = perp_component(vImpact, thePlane.Normal) - (theSphere.getCoR() * parallel_component(vImpact, thePlane.Normal));

            float tRemaining = Time.deltaTime - tImpact;

            Vector3 v = vImpactOut + theSphere.getAcceleration() * tRemaining; // v is velocity
            Vector3 position = pImpact + v * tRemaining;

            theSphere.setVelocity(v);
            transform.position = position;
        }
        theSphere.setd0(d1);
    }

    public static float parallel_Distance(Vector3 v, Vector3 n)
    {
        return Vector3.Dot(v, n.normalized);
    }
    public static Vector3 parallel_component(Vector3 v, Vector3 n)
    {
        return (Vector3.Dot(v, n.normalized) * n.normalized);
    }
    public static Vector3 perp_component(Vector3 v, Vector3 n)
    {
        return v - parallel_component(v, n);
    }

}
