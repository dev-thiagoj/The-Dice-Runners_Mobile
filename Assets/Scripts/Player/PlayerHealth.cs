using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] bool _isAlive = true;
    [SerializeField] PlayerController player;
    [SerializeField] PlayerPopUpManager playerPopUp;
    [SerializeField] PlayerAnimationManager playerAnimations;

    private void OnValidate()
    {
        player = GetComponent<PlayerController>();
        playerPopUp = GetComponent<PlayerPopUpManager>();
        playerAnimations = GetComponent<PlayerAnimationManager>();
    }

    private void OnEnable()
    {   
        Actions.onDeadPlayer += Dead;
    }

    private void OnDisable()
    {
        Actions.onDeadPlayer -= Dead;
    }

    private void Update()
    {
        if (!_isAlive && player.IsGrounded()) Dead();
    }

    public void Dead()
    {
        if (_isAlive)
        {
            _isAlive = false;
            player.ChangeCanRunValue();
            player.characterController.detectCollisions = false;
            OnDead();
        }
    }

    public void OnDead()
    {
        playerPopUp.CallExpression(ExpressionType.DEATH);
        SFXPool.Instance.Play(SFXType.DEATH_03);
        playerAnimations.SetTriggerByString("Die");
        Invoke(nameof(ShowEndGameScreen), 5);
    }

    public void ShowEndGameScreen()
    {
        GameManager.Instance.EndGame();
    }
}
