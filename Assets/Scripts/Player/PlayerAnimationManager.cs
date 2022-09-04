using UnityEngine;

public class PlayerAnimationManager : MonoBehaviour
{
    public Animator animator;

    [Header("Look At EndGame")]
    public RotationLookAt rotationLook;

    private void OnValidate()
    {
        if (rotationLook == null) rotationLook = GetComponent<RotationLookAt>();
    }

    private void Awake()
    {
        Actions.findFemaleAnim += FindLookAtTarget;
        Actions.onFinishLine += ReachedFinishLine;
    }

    void FindLookAtTarget()
    {
        rotationLook.target = GameObject.Find("CharacterPos").GetComponent<Transform>();
    }

    // Achar o animator que ser� usado ap�s a escolha do personagem
    public void FindAnimator()
    {
        animator = GetComponentInChildren<Animator>();
    }

    // Acionar a anima��o por trigger
    public void SetTriggerByString(string triggerName)
    {
        animator.SetTrigger(triggerName);
    }

    // Definir a velocidade da anima��o
    public void SetAnimationSpeed(float speed)
    {
        animator.speed = speed;
    }

    void ReachedFinishLine()
    {
        SetTriggerByString("Idle");
    }
}