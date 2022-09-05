using System.Collections;
using UnityEngine;
using Singleton;
using DG.Tweening;

public class PlayerController : Singleton<PlayerController> // <------- deixar de usar singleton e usar os actions
{
    #region === VARIABLES ===

    public CharacterController characterController;
    public PlayerAnimationManager playerAnimation;
    public PlayerPopUpManager playerPopUp;

    [Header("Movement")]
    public float runSpeed = 5;
    public float sideSpeed = 5;
    public float currRunSpeed;
    float _currSideSpeed;
    [Range(1, 4)]
    public float walkSpeed = 3;
    public bool canRun = false;

    [Header("Jump")]
    [SerializeField] float jumpForce = 8;
    [SerializeField] public float gravity = 9.8f;
    float _vSpeed;
    float _currJumpForce;
    float distToGround;
    float spaceToGround = .3f;

    [Header("Turbo PowerUp")] // <---------- srp
    //public float turboSpeed;
    //public int maxTurbos = 3;
    //public int _currTurbo;
    //public float turboTime = 2;
    //bool _turboOn = false;

    [Header("Magnetic Powerup")] // <------------------- srp
    public Transform magneticCollider;
    public ForceFieldManager forceField;
    public float magneticSize;
    public float magneticTime;
    public bool _hasMagnetic = false;

    [Header("Bounds")]
    private float range = 5.6f;

    internal PlayerInputSystem _playerInputs;
    private bool _isAlive = true;
    #endregion

    private void OnValidate()
    {
        if (playerAnimation == null) playerAnimation = GetComponent<PlayerAnimationManager>();
        if (playerPopUp == null) playerPopUp = GetComponent<PlayerPopUpManager>();
        if (forceField == null) forceField = GetComponentInChildren<ForceFieldManager>();
    }

    #region === OnEnable/Disable ===

    private void OnEnable()
    {
        _playerInputs.Enable();
    }

    private void OnDisable()
    {
        _playerInputs.Disable();
    }

    #endregion

    protected override void Awake()
    {
        base.Awake();

        _playerInputs = new PlayerInputSystem();

        _playerInputs.Gameplay.Jump.performed += ctx => Jump();
        _playerInputs.Gameplay.Stop.performed += ctx => Walk();
        _playerInputs.Gameplay.Stop.canceled += ctx => BackRun();
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        currRunSpeed = runSpeed;
        _currSideSpeed = sideSpeed;
        _currJumpForce = jumpForce;
    }

    // Update is called once per frame
    void Update()
    {
        if (canRun)
        {
            IsGrounded();
            Movement();
            Bounds();
        }

        if (!canRun) playerAnimation.SetTriggerByString("Idle");
        if (canRun && IsGrounded()) playerAnimation.SetTriggerByString("Run");
        if (!_isAlive && IsGrounded()) Dead();
    }

    #region === MOVEMENTS ===

    public void InvokeStartRun() // <----------- resolver com actions
    {
        Invoke(nameof(StartRun), 5);
        Invoke(nameof(StartExpressions), 4);
    }

    public void StartExpressions()
    {
        playerPopUp.CallExpression(ExpressionType.SURPRISE);
    }

    public void StartRun()
    {
        canRun = true;
    }

    // New Input System _ Mobile
    public void Movement()
    {
        Vector2 movement = _playerInputs.Gameplay.Move.ReadValue<Vector2>();
        Vector3 move = new Vector3((movement.x * -_currSideSpeed), 0, currRunSpeed);

        _vSpeed -= gravity * Time.deltaTime;
        move.y = _vSpeed;

        characterController.Move(move * Time.deltaTime);
    }

    public void Jump()
    {
        if (characterController.isGrounded)
        {
            _vSpeed = _currJumpForce;
            _currSideSpeed = 0;
            playerAnimation.SetTriggerByString("Jump");
            SFXPool.Instance.Play(SFXType.JUMP_02);
            Invoke(nameof(BackRun), 2);
        }
    }

    public void Walk()
    {
        currRunSpeed = walkSpeed;
        _currSideSpeed = walkSpeed;
        _currJumpForce = 0;
        playerAnimation.SetAnimationSpeed(.5f);
    }

    public void BackRun()
    {
        currRunSpeed = runSpeed;
        _currSideSpeed = sideSpeed;
        _currJumpForce = jumpForce;
        playerAnimation.SetAnimationSpeed(1);
    }
    bool IsGrounded()
    {
        Debug.DrawRay(transform.position, -Vector2.up, Color.magenta, distToGround + spaceToGround);
        return Physics.Raycast(transform.position, -Vector2.up, distToGround + spaceToGround);
    }

    void Bounds()
    {
        if (transform.position.x > range)
        {
            characterController.transform.position = new Vector3(range, transform.position.y, transform.position.z);

        }
        else if (transform.position.x < -range)
        {
            characterController.transform.position = new Vector3(-range, transform.position.y, transform.position.z);
        }
    }
    #endregion

    #region === HEALTH ===
    public void Dead()
    {
        if (_isAlive)
        {
            _isAlive = false;
            canRun = false;
            characterController.detectCollisions = false;
            playerPopUp.CallExpression(ExpressionType.DEATH); // <--------- criar um action que chama todos os efeitos de morte?
            OnDead();
        }
    }

    public void OnDead()
    {
        SFXPool.Instance.Play(SFXType.DEATH_03);
        playerAnimation.SetTriggerByString("Die");
        Invoke(nameof(ShowEndGameScreen), 5);
    }

    public void ShowEndGameScreen()
    {
        GameManager.Instance.EndGame();
    }
    #endregion

    #region === POWERUPS ===
    /*public void TurboPlayer()
    {
        if (_currTurbo < maxTurbos && characterController.isGrounded)
        {
            StartCoroutine(TurboCoroutine());
            _currTurbo++;
            ItemManager.Instance.RemoveTurbo();
        }
        else ItemManager.Instance.WithoutTurboWarning();
    }

    public IEnumerator TurboCoroutine()
    {
        if (!_turboOn)
        {
            _turboOn = true;
            _currRunSpeed = turboSpeed;
            SFXPool.Instance.Play(SFXType.USE_TURBO_06);
            yield return new WaitForSeconds(turboTime);
        }

        _currRunSpeed = runSpeed;
        _turboOn = false;
        StopCoroutine(TurboCoroutine());
    }*/

    public void MagneticOn(bool b = false)
    {
        if (b == true)
        {
            StartCoroutine(MagneticCoroutine());
            forceField.StartParticleField();
            SFXPool.Instance.Play(SFXType.USE_MAGNETIC_08);
        }

    }

    public IEnumerator MagneticCoroutine()
    {
        magneticCollider.transform.DOScaleX(5, 1);
        yield return new WaitForSeconds(magneticTime);
        magneticCollider.transform.DOScaleX(1, 1);
        MagneticOn(false);
        StopCoroutine(MagneticCoroutine());
    }
    #endregion
}