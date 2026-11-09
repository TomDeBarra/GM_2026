
using UnityEngine;

public class SnowballHandFollow : MonoBehaviour
{
    private Transform hand;

    void LateUpdate()
    {
        // Stop following once detached from player.
        if (transform.parent == null) return;

        // Your existing hand-follow code below...

        // Find the hand of the player we're attached to.
        if (hand == null)
        {
            if (transform.parent == null)
                return;

            Animator animator =
                transform.parent.GetComponentInChildren<Animator>();

            if (animator == null || !animator.isHuman)
                return;

            hand = animator.GetBoneTransform(
                HumanBodyBones.RightHand
            );
        }

        // Keep the snowball at the hand's position.
        if (hand != null)
        {
            transform.position = hand.position;
        }
    }
}
