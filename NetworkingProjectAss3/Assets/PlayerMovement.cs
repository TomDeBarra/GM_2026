
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;

public class PlayerMovement : NetworkBehaviour
{
    public float walkSpeed = 2f;
    public float runSpeed = 5f;
    public float turnSpeed = 10f;

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!IsOwner) return;

        Keyboard keyboard = Keyboard.current;
        if (keyboard == null) return;

        if (keyboard.eKey.wasPressedThisFrame)
        {
            animator.SetTrigger("Pickup");
        }

        float x = 0f;
        float z = 0f;

        if (keyboard.wKey.isPressed) z += 1f;
        if (keyboard.sKey.isPressed) z -= 1f;
        if (keyboard.aKey.isPressed) x -= 1f;
        if (keyboard.dKey.isPressed) x += 1f;

        Vector3 direction = new Vector3(x, 0, z).normalized;

        bool moving = direction.magnitude > 0;
        bool running = moving && keyboard.leftShiftKey.isPressed;

        if (moving)
        {
            float speed = running ? runSpeed : walkSpeed;

            transform.position += direction * speed * Time.deltaTime;

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(direction),
                turnSpeed * Time.deltaTime
            );
        }

        float animationSpeed = 0f;

        if (moving)
            animationSpeed = running ? 1f : 0.5f;

        animator.SetFloat("Speed", animationSpeed, 0.15f, Time.deltaTime);
    }
}
