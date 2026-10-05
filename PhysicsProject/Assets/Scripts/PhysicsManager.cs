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
    ArrayList physicsObjectList = new ArrayList();

    public PlanePhysics thePlane;
    public SpherePhysics sphere1;
    public SpherePhysics sphere2;

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
    }

    // Move the sphere's collision methods here
}
