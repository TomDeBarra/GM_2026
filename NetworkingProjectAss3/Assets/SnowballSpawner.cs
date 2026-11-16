
using UnityEngine;
using Unity.Netcode;
using UnityEngine.InputSystem;
using System.Collections;

public class SnowballSpawner : NetworkBehaviour //implementing network behaviour is identical to monobehaviour but allows rpc calls etc...
{
    [SerializeField] private GameObject snowballPrefab;
    [SerializeField] private float throwForce = 3f;

    [SerializeField] private float pickupRange = 5f;
    [SerializeField] private float pickupTime = 1f;

    private bool isPickingUp = false;
    private NetworkObject heldSnowball;

    Color randomColour;

    void Update()
    {
        if (!IsOwner || Keyboard.current == null) return;

        if (Keyboard.current.rKey.wasPressedThisFrame)
            SpawnSnowballRpc();

        if (Keyboard.current.tKey.wasPressedThisFrame)
            ThrowSnowballRpc(transform.forward);

        if (Keyboard.current.eKey.wasPressedThisFrame)
            PickupSnowballRpc();
    }

    void Start()
    {
        randomColour = Random.ColorHSV(0f, 1f, 0.7f, 1f, 0.8f, 1f);
    }
    [Rpc(SendTo.Server)]
    private void SpawnSnowballRpc()
    {
        if (heldSnowball != null ||
            snowballPrefab == null) return;

        GameObject snowball = Instantiate(snowballPrefab);

        NetworkObject netObj =
            snowball.GetComponent<NetworkObject>();

        netObj.Spawn(); // network manager will check prefab list and because it has a prefab list with snowballs it can spawn snowballs for everybody

       

        SetSnowballColourRpc(netObj, randomColour);

        if (netObj.TrySetParent(NetworkObject, false)) // NetworkObject is the player prefab to which this script, snowball spawner is attached
        {
            heldSnowball = netObj;
        }
        // Now script on snowball, find hand, will follow the hand of its new parent
        else
        {
            netObj.Despawn(true);
        }
    }

    [Rpc(SendTo.Everyone)]
    private void SetSnowballColourRpc(NetworkObjectReference snowballRef, Color colour)
    {
        if (!snowballRef.TryGet(out NetworkObject snowball))
            return;

        Renderer renderer = snowball.GetComponent<Renderer>();

        if (renderer != null)
            renderer.material.color = colour;
    }

    [Rpc(SendTo.Server)]
    private void ThrowSnowballRpc(Vector3 direction)
    {
        if (heldSnowball == null) return;

        if (!heldSnowball.TryRemoveParent(true))
            return;

        Rigidbody rb =
            heldSnowball.GetComponent<Rigidbody>();

        rb.isKinematic = false;
        rb.useGravity = true;

        rb.AddForce(direction.normalized * throwForce + Vector3.up * 3f, // vectorup * 3f adds 3y to the throw added to help prevent clipping through plane
                    ForceMode.Impulse); // AddForce must be passed in ForceMode as a second parameter, impulse means instantly apply the force, Force means over time like e.g. pushing

        heldSnowball = null;
    }

    [Rpc(SendTo.Server)]
    private void PickupSnowballRpc()
    {
        if (heldSnowball != null || isPickingUp)
            return;

        StartCoroutine(PickupAfterDelay());
    }

    private IEnumerator PickupAfterDelay()
    {
        isPickingUp = true;

        yield return new WaitForSeconds(pickupTime);

        if (heldSnowball != null)
        {
            isPickingUp = false;
            yield break;
        }

        Collider[] nearby =
            Physics.OverlapSphere(transform.position, pickupRange);

        NetworkObject nearest = null;
        float nearestDistance = pickupRange;

        foreach (Collider hit in nearby)
        {
            NetworkObject ball =
                hit.GetComponentInParent<NetworkObject>();

            if (ball == null || !ball.IsSpawned)
                continue;

            if (ball.GetComponent<SnowballHandFollow>() == null)
                continue;

            if (ball.transform.parent != null)
                continue;

            float distance = Vector3.Distance(
                transform.position, ball.transform.position
            );

            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearest = ball;
            }
        }

        if (nearest != null)
        {
            if (nearest.TrySetParent(NetworkObject, false))
            {
                Rigidbody rb =
                    nearest.GetComponent<Rigidbody>();

                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.isKinematic = true;
                rb.useGravity = false;

                heldSnowball = nearest;
            }
        }

        isPickingUp = false;
    }
}
