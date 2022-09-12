using UnityEngine;

public class EndGameAnimation : MonoBehaviour
{
    Animator animator;

    private void OnEnable() { Actions.onFinishLine += GetAnimatorInScene; }

    private void OnDisable() { Actions.onFinishLine -= GetAnimatorInScene; }

    public void GetAnimatorInScene()
    {
        animator = GetComponentInChildren<Animator>();
        PlayEndLevelAnim();
    }

    void PlayEndLevelAnim()
    { 
        animator.SetTrigger("LevelWin");
    }
}
