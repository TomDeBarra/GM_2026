
// I'm aware this isn't being used in assignment 3 at present, however I kept it here incase I'm told assignment 3 needs to carry over kinematics from assignment 2

using UnityEngine;

public class SnowballIK : MonoBehaviour
{
    public Transform snowball;

    private Animator animator;
    private Transform upperArm;
    private Transform lowerArm;
    private Transform hand;
    private int pickupLayer;

    void Start()
    {
        animator = GetComponent<Animator>();

        upperArm = animator.GetBoneTransform(HumanBodyBones.RightUpperArm);
        lowerArm = animator.GetBoneTransform(HumanBodyBones.RightLowerArm);
        hand = animator.GetBoneTransform(HumanBodyBones.RightHand);

        pickupLayer = animator.GetLayerIndex("PickupLayer");
    }

    void LateUpdate()
    {
        if (snowball == null || pickupLayer < 0) return;

        if (Vector3.Distance(transform.position, snowball.position) > 1f)
            return;

        if (!animator.GetCurrentAnimatorStateInfo(pickupLayer)
            .IsName("PickupLayer.Pickup"))
            return;

        // Repeatedly adjust elbow and shoulder 8 times each frame
        // Cyclic Coordinate Descent (CCD)
        for (int i = 0; i < 8; i++)
        {
            RotateTowardsTarget(lowerArm);
            RotateTowardsTarget(upperArm);
        }
    }

    void RotateTowardsTarget(Transform joint)
    {
        Vector3 toHand = hand.position - joint.position;
        Vector3 toTarget = snowball.position - joint.position;

        if (toHand.sqrMagnitude < 0.0001f ||
            toTarget.sqrMagnitude < 0.0001f)
            return;

        Quaternion rotation = Quaternion.FromToRotation(
            toHand, toTarget
        );

        joint.rotation = rotation * joint.rotation;
    }
}
