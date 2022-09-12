using UnityEngine;

public class PlayerAnimationManager : MonoBehaviour
{
    public Animator animator;

    [Header("Look At EndGame")]
    RotationLookAt rotationLook;

    private void OnValidate()
    {
        if (rotationLook == null) rotationLook = GetComponent<RotationLookAt>();
    }

    private void OnEnable()
    {
        Actions.findEndLevelAnim += FindTargetToLookAtInTheEnd;
        Actions.onFinishLine += ReachedFinishLine;
    }

    private void OnDisable()
    {
        Actions.findEndLevelAnim -= FindTargetToLookAtInTheEnd;
        Actions.onFinishLine -= ReachedFinishLine;
    }

    // Achar a posição do personagem do final do level
    void FindTargetToLookAtInTheEnd()
    {
        rotationLook.FindPlayerTarget();
    }

    // Achar o animator que será usado após a escolha do personagem
    public void FindAnimator()
    {
        animator = GetComponentInChildren<Animator>();
    }

    // Acionar a animação por trigger
    public void SetTriggerByString(string triggerName)
    {
        animator.SetTrigger(triggerName);
    }

    // Definir a velocidade da animação
    public void SetAnimationSpeed(float speed)
    {
        animator.speed = speed;
    }

    void ReachedFinishLine()
    {
        SetTriggerByString("Idle");
        Invoke(nameof(PlayWinLevelAnim), 1);
    }

    void PlayWinLevelAnim()
    {
        SetTriggerByString("LevelWin");
    }
}