
using UnityEngine;
using Unity.Netcode;
using UnityEngine.InputSystem;

public class SnowballSpawner : NetworkBehaviour
{
    [SerializeField] private GameObject snowballPrefab;
    [SerializeField] private float throwForce = 3f;

    private bool hasSpawned = false;
    private NetworkObject spawnedSnowball;

    void Update()
    {
        if (!IsOwner) return;
        if (Keyboard.current == null) return;

        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            SpawnSnowballRpc();
        }

        if (Keyboard.current.tKey.wasPressedThisFrame)
        {
            ThrowSnowballRpc(transform.forward);
        }
    }

    [Rpc(SendTo.Server)]
    private void SpawnSnowballRpc()
    {
        if (hasSpawned || snowballPrefab == null) return;

        GameObject snowball = Instantiate(snowballPrefab);

        NetworkObject netObj =
            snowball.GetComponent<NetworkObject>();

        netObj.Spawn();

        if (netObj.TrySetParent(NetworkObject, false))
        {
            spawnedSnowball = netObj;
            hasSpawned = true;
        }
        else
        {
            netObj.Despawn(true);
        }
    }

    [Rpc(SendTo.Server)]
    private void ThrowSnowballRpc(Vector3 direction)
    {
        if (spawnedSnowball == null) return;

        // Detach from player, keeping world position.
        if (!spawnedSnowball.TryRemoveParent(true))
            return;

        Rigidbody rb =
            spawnedSnowball.GetComponent<Rigidbody>();

        rb.isKinematic = false;
        rb.useGravity = true;

        // Throw in the player's facing direction.
        rb.AddForce(direction.normalized * throwForce,
                    ForceMode.Impulse);

        spawnedSnowball = null;
    }
}
