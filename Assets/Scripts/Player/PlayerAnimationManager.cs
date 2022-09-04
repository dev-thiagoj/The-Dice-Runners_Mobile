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
    }
}