using UnityEngine;

public class PlayerAnimationManager : MonoBehaviour
{
    public static Animator animator;

    public void FindAnimator()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void SetTriggerByString(string trigger)
    {
        animator.SetTrigger(trigger);
    }

    public void SetAnimationSpeed(float speed)
    {
        animator.speed = speed;
    }
}