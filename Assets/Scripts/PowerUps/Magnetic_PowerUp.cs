using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnetic_PowerUp : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform magneticTrigger;
    [SerializeField] ForceFieldManager forceField;
    [SerializeField] PlayerController player;

    [Header("SETUP")]
    public int magneticValue;
    [SerializeField] float magneticSize;
    [SerializeField] float magneticTime;
    [SerializeField] bool _inUse;

    [Header("Particle System")]
    [SerializeField] ParticleSystem playerParticleSystem;

    [Header("Actions")]
    public static Action onChangeAmount;

    private void OnEnable() 
    { 
        Actions.onFinishLine += SaveMagneticsValueAtTheEnd;
        ItemCollectableMagnetic.onMagneticCollect += AddMagnetic;
    }

    private void OnDisable() 
    { 
        Actions.onFinishLine -= SaveMagneticsValueAtTheEnd;
        ItemCollectableMagnetic.onMagneticCollect -= AddMagnetic;
    }

    private void OnValidate()
    {
        if (forceField == null) forceField = GetComponentInChildren<ForceFieldManager>();
        if (playerParticleSystem == null) playerParticleSystem = forceField.gameObject
                .GetComponentInChildren<ParticleSystem>();
    }

    protected void Awake()
    { 
        if (player == null) player = GetComponent<PlayerController>(); 
    }

    protected void Start()
    { 
        player._playerInputs.Gameplay.Magnetic.performed += ctx => StartMagnetic(); 
    }

    void AddMagnetic()
    {
        magneticValue++;
        onChangeAmount.Invoke();
    }

    public bool HasMagnetic()
    {
        if (magneticValue > 0)
        {
            return true;
        }
        return false;
    }

    public void StartMagnetic()
    {
        if (HasMagnetic())
        {
            if (!_inUse)
            {
                StartCoroutine(MagneticCoroutine());
                playerParticleSystem.Play();
                SFXPool.Instance.Play(SFXType.USE_MAGNETIC_08);
                magneticValue--;
                onChangeAmount.Invoke();
            }
        }
    }

    public IEnumerator MagneticCoroutine()
    {
        _inUse = !_inUse;
        magneticTrigger.transform.DOScaleX(5, 1);
        yield return new WaitForSeconds(magneticTime);
        magneticTrigger.transform.DOScaleX(1, 1);
        _inUse = !_inUse;
        StopCoroutine(MagneticCoroutine());
    }

    void SaveMagneticsValueAtTheEnd()
    {
        PlayerPrefs.SetInt("magneticsValue", magneticValue);
    }
}
