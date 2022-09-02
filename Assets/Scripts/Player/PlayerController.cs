using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Singleton;
using DG.Tweening;

public class PlayerController : Singleton<PlayerController> // <------- deixar de usar singleton e usar os actions
{
    #region === VARIABLES ===

    public CharacterController characterController;
    public PlayerAnimationManager playerAnimation;
    public AudioSource audioSource; // <------------ srp
    public List<AudioClip> sfxPlayer; // <----------- srp

    [Header("Inputs")]
    public PlayerInputSystem playerInputs; // <-------------- srp

    [Header("Movement")]
    public float runSpeed = 5;
    public float sideSpeed = 5;
    float _currRunSpeed;
    float _currSideSpeed;
    [Range(1, 4)]
    public float walkSpeed = 3;
    public bool canRun = false;

    [Header("Jump")]
    float _vSpeed;
    public float jumpForce = 8;
    float _currJumpForce;
    public float gravity = 9.8f;
    float distToGround;
    float spaceToGround = .3f;

    [Header("Turbo PowerUp")] // <---------- srp
    public float turboSpeed;
    public int maxTurbos = 3;
    public int _currTurbo;
    public float turboTime = 2;
    bool _turboOn = false;

    [Header("Magnetic Powerup")] // <------------------- srp
    public Transform magneticCollider;
    public ForceFieldManager forceField;
    public float magneticSize;
    public float magneticTime;
    public bool _hasMagnetic = false;

    [Header("Bounds")]
    private float range = 5.6f;

    [Header("Expressions")] // <-------------- srp
    public Transform[] expressions;
    public float animationDuration;
    public Ease ease;

    [Header("Look At EndGame")] // <-------------------- srp
    public RotationLookAt rotationLook;
    public string targetName;

    public bool _isAlive = true;
    #endregion

    private void OnValidate()
    {
        if (playerAnimation == null) playerAnimation = GetComponent<PlayerAnimationManager>();
        if (audioSource == null) audioSource = GetComponentInChildren<AudioSource>();
        if (rotationLook == null) rotationLook = GetComponent<RotationLookAt>();
        if (forceField == null) forceField = GetComponentInChildren<ForceFieldManager>();
    }

    #region === OnEnable/Disable ===

    private void OnEnable()
    {
        playerInputs.Enable();
        Actions.findFemaleAnim += FindLookAtTarget;
    }

    private void OnDisable()
    {
        playerInputs.Disable();
        Actions.findFemaleAnim -= FindLookAtTarget;
    }

    #endregion

    protected override void Awake()
    {
        base.Awake();

        playerInputs = new PlayerInputSystem();

        playerInputs.Gameplay.Jump.performed += ctx => Jump();
        playerInputs.Gameplay.Turbo.performed += ctx => TurboPlayer();

        playerInputs.Gameplay.Stop.performed += ctx => Walk();
        playerInputs.Gameplay.Stop.canceled += ctx => BackRun();

    }

    // Start is called before the first frame update
    void Start()
    {
        _currRunSpeed = runSpeed;
        _currSideSpeed = sideSpeed;
        _currJumpForce = jumpForce;
        _currTurbo = 0;
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

    void FindLookAtTarget() // <------------------ Criar classe específica de procurar objetos na cena
    {
        rotationLook.target = GameObject.Find("CharacterPos").GetComponent<Transform>();
    }

    #region === MOVEMENTS ===

    public void InvokeStartRun() // <----------- resolver com actions
    {
        Invoke(nameof(StartRun), 5);
        Invoke(nameof(StartExpressionsCoroutine), 4);
    }

    public void StartExpressionsCoroutine() // <---------------- criar classe
    {
        StartCoroutine(ExpressionsCoroutine(0));
    }

    public void StartRun()
    {
        canRun = true;
        //Actions.startTutorial();
    }

    // New Input System _ Mobile
    public void Movement()
    {
        Vector2 movement = playerInputs.Gameplay.Move.ReadValue<Vector2>();
        Vector3 move = new Vector3((movement.x * -_currSideSpeed), 0, _currRunSpeed);

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
        _currRunSpeed = walkSpeed;
        _currSideSpeed = walkSpeed;
        _currJumpForce = 0;
        playerAnimation.SetAnimationSpeed(.5f);
    }

    public void BackRun()
    {
        _currRunSpeed = runSpeed;
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
        _isAlive = true;
        canRun = false;
        characterController.detectCollisions = false;
        StartCoroutine(ExpressionsCoroutine(1));
        OnDead();
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
    public void TurboPlayer()
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
    }

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

    #region === EXPRESSIONS ===

    public IEnumerator ExpressionsCoroutine(int index)
    {
        expressions[index].transform.DOScale(0.5f, animationDuration).SetLoops(2, LoopType.Yoyo).SetEase(ease);
        yield return new WaitForEndOfFrame();
    }

    #endregion
}