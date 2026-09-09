using UnityEngine;

public class SpherePhysics : MonoBehaviour
{   
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

        if(transform.position.y < 0.05f) // detect collision
            transform.position -= velocity * Time.deltaTime;
            velocity = -CoR*velocity;
    }
}
