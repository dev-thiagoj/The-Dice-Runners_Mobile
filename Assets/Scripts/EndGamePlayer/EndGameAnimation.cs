using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameAnimation : MonoBehaviour
{
    Animator animator;

    private void Start()
    {
        Actions.onFinishLine += GetAnimatorInScene;
    }

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
