using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PhysicsManager : MonoBehaviour
{
    public static PhysicsManager Instance;
    List<ICollidable> allCollidables;

    void Awake()
    {
        Instance = this;
    }

    public void Register(ICollidable collidable)
    {
        if (!allCollidables.Contains(collidable))
        {
            allCollidables.Add(collidable);
        }
    }

    void Start()
    {
         allCollidables = 
              FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
                                       .OfType<ICollidable>()
                                       .ToList();
    }

    void Update()
    {

        for (int i = 0; i < allCollidables.Count; i++)
        {
            for (int j = i+1; j < allCollidables.Count; j++)
            {
                if (allCollidables[i].collidingWith(allCollidables[j]))
                {
                    allCollidables[i].resolve(allCollidables[j]);
                }
            }
        }
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
