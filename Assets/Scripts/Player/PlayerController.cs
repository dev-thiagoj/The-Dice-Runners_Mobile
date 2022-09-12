using System.Collections;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
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
    public Vector3 currPosition;

    [Header("Jump")]
    [SerializeField] float jumpForce = 8;
    [SerializeField] public float gravity = 9.8f;
    float _vSpeed;
    float distToGround;
    float spaceToGround = .3f;

    [Header("Bounds")]
    private float range = 5.6f;

    internal PlayerInputSystem _playerInputs;
    #endregion

    private void OnValidate()
    {
        if (playerAnimation == null) playerAnimation = GetComponent<PlayerAnimationManager>();
        if (playerPopUp == null) playerPopUp = GetComponent<PlayerPopUpManager>();
    }

    #region === OnEnable/Disable ===

    private void OnEnable()
    {
        _playerInputs.Enable();
        Actions.onFinishLine += ChangeCanRunValue;
        Actions.onGameStarted += InvokeStartRun;
    }

    private void OnDisable()
    {
        _playerInputs.Disable();
        Actions.onFinishLine -= ChangeCanRunValue;
        Actions.onGameStarted -= InvokeStartRun;
    }

    #endregion

    protected void Awake()
    {
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
    }

    // Update is called once per frame
    void Update()
    {
        currPosition = transform.position;

        if (canRun)
        {
            IsGrounded();
            Movement();
            Bounds();
        }

        if (canRun && IsGrounded()) playerAnimation.SetTriggerByString("Run");
    }

    #region === MOVEMENTS ===

    public void InvokeStartRun() // <----------- resolver com actions
    {
        Invoke(nameof(ChangeCanRunValue), 5);
        Invoke(nameof(StartExpressions), 4);
    }

    public void StartExpressions()
    {
        playerPopUp.CallExpression(ExpressionType.SURPRISE);
    }

    public void ChangeCanRunValue()
    {
        canRun = !canRun;
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
            _vSpeed = jumpForce;
            playerAnimation.SetTriggerByString("Jump");
            SFXPool.Instance.Play(SFXType.JUMP_02);
            Invoke(nameof(BackRun), 2);
        }
    }

    public void Walk()
    {
        currRunSpeed = walkSpeed;
        _currSideSpeed = walkSpeed;
        playerAnimation.SetAnimationSpeed(.5f);
    }

    public void BackRun()
    {
        currRunSpeed = runSpeed;
        _currSideSpeed = sideSpeed;
        playerAnimation.SetAnimationSpeed(1);
    }

    public bool IsGrounded()
    {
        Debug.DrawRay(transform.position, -Vector2.up, Color.magenta, distToGround + spaceToGround);
        return Physics.Raycast(transform.position, -Vector2.up, distToGround + spaceToGround);
    }

    void Bounds()
    {
        if (transform.position.x > range)
        {
            transform.position = new Vector3(range, transform.position.y, transform.position.z);
        }
        else if (transform.position.x < -range)
        {
            transform.position = new Vector3(-range, transform.position.y, transform.position.z);
        }
    }
    #endregion
}