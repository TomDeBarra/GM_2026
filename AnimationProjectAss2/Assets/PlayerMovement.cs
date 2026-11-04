
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
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

        // magnitude means length of vector, is it greater than 0?
        bool moving = direction.magnitude > 0;
        bool running = moving && keyboard.leftShiftKey.isPressed;

        // Movement and rotation
        if (moving)
        {
            float speed = running ? runSpeed : walkSpeed;

            transform.position += direction * speed * Time.deltaTime;

            // Quaternion.Slerp(startRotation, targetRotation, t);
            // Slerp makes things rotate smoothly
            // t = how quickly we should rotate from start to target

            // A quaternion looks something like q = w + xi + yj +zk
            // Complicated calculation unity uses to calculate rotations, ijk are imaginary dimensions
            // transform.rotation for example is a quaternion

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(direction),
                turnSpeed * Time.deltaTime
            );
        }

        // Blend Tree animation
        float animationSpeed = 0f;

        if (moving)
            animationSpeed = running ? 1f : 0.5f;

        animator.SetFloat("Speed", animationSpeed, 0.15f, Time.deltaTime);
    }
}
