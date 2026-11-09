
using UnityEngine;

public class SnowballHandFollow : MonoBehaviour
{
    private Transform hand;
    private Transform lastParent;

    void LateUpdate()
    {
        // Detect when the snowball changes owner/parent.
        if (transform.parent != lastParent)
        {
            lastParent = transform.parent;
            hand = null;
        }

        // If thrown, stop following any hand.
        if (transform.parent == null) return;

        if (hand == null)
        {
            Animator animator =
                transform.parent.GetComponentInChildren<Animator>();

            if (animator == null || !animator.isHuman)
                return;

            hand = animator.GetBoneTransform(
                HumanBodyBones.RightHand
            );
        }

        if (hand != null)
            transform.position = hand.position;
    }
}
