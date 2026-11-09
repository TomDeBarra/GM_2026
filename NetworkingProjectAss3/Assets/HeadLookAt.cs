
using UnityEngine;

public class HeadLookAt : MonoBehaviour
{
    public Transform target;

    private Transform head;

    void Start()
    {
        Animator animator = GetComponent<Animator>();
        head = animator.GetBoneTransform(HumanBodyBones.Head);
    }

    void LateUpdate()
    {
        if (head != null && target != null)
        {
            head.LookAt(target);
        }
    }
}
