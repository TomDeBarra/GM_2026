using UnityEngine;

public class SphereSpawner : MonoBehaviour
{
    void Update()
    {
        float targetX = 0f;
        float targetZ = 0f;

        bool triggerSpawn = false;

        // Check number keys (Alpha for top row, Keypad for numpad)
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1)) { targetX = -0f; targetZ = -7f; triggerSpawn = true; }
        if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2)) { targetX = -0f; targetZ = -3f; triggerSpawn = true; }
        if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3)) { targetX = -0f; targetZ = 0f; triggerSpawn = true; }
        if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4)) { targetX = -0f; targetZ = 3f; triggerSpawn = true; }
        if (Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown(KeyCode.Keypad5)) { targetX = -0f; targetZ = 7f; triggerSpawn = true; }

        if (triggerSpawn)
        {
            SpawnSphere(targetX, targetZ);
        }
    }
    void SpawnSphere(float xCoord, float zCoord)
    {
        // 1. Create a default Unity sphere primitive
        GameObject newSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);

        // 2. Set position (X based on key, Y = 5, Z = 0) and scale (1,1,1)
        newSphere.transform.position = new Vector3(xCoord, 3f, zCoord);
        newSphere.transform.localScale = new Vector3(3f, 3f, 3f);  // newSphere.transform.localScale = Vector3.one;

        // 3. Attach your custom SpherePhysics script (mass will default to 1)
        SpherePhysics physicsComponent = newSphere.AddComponent<SpherePhysics>();

        // 4. Set the mass variable to 3
        physicsComponent.mass = 3f;

        Debug.Log($"Spawned Sphere at X: {xCoord}, Y: 5");

        // Instantly register it with the manager without scanning the whole scene!
        if (PhysicsManager.Instance != null)
        {
            PhysicsManager.Instance.Register(physicsComponent);
        }
    }
}
