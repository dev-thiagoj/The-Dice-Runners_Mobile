using UnityEngine;

public class PlayerAnimationManager : MonoBehaviour
{
    public Animator animator;

    public void FindAnimator()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void SetTriggerByString(string triggerName)
    {
        animator.SetTrigger(triggerName);
    }

    public void SetAnimationSpeed(float speed)
    {
        animator.speed = speed;
    }
}