using UnityEngine;

// 메서드만들기
// 정적 클래스여야 함
public static class Extension
{
    public static void SetIKPositionAndWeight(this Animator animator, AvatarIKGoal goal, Vector3 goalPosition, float weight = 1f)
    {
        animator.SetIKPositionWeight(goal, weight);
        animator.SetIKPosition(goal, goalPosition);
    }

    public static void SetIKRotationAndWeight(this Animator animator, AvatarIKGoal goal, Quaternion goalQuaternion, float weight = 1f)
    {
        animator.SetIKRotationWeight(goal, weight);
        animator.SetIKRotation(goal, goalQuaternion);
    }
}
