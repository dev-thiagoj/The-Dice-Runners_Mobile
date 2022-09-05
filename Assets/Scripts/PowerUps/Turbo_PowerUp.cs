using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turbo_PowerUp : MonoBehaviour
{
    public float turboSpeed;
    public int turbosAmount = 3;
    public int currTurbo;
    public float turboDuration = 2;
    bool _turboOn = false;

    PlayerController player;

    protected void Awake()
    {
        player = GetComponent<PlayerController>();
    }

    protected void Start()
    {
        player._playerInputs.Gameplay.Turbo.performed += ctx => UseTurbo();
        currTurbo = 0;
    }

    public void UseTurbo()
    {
        if (currTurbo < turbosAmount && player.characterController.isGrounded)
        {
            StartCoroutine(TurboCoroutine());
            currTurbo++;
            ItemManager.Instance.RemoveTurbo();
        }
        else ItemManager.Instance.WithoutTurboWarning();
    }

    public IEnumerator TurboCoroutine()
    {
        if (!_turboOn)
        {
            _turboOn = true;
            player.currRunSpeed = turboSpeed;
            SFXPool.Instance.Play(SFXType.USE_TURBO_06);
            yield return new WaitForSeconds(turboDuration);
        }

        player.currRunSpeed = player.runSpeed;
        _turboOn = false;
        StopCoroutine(TurboCoroutine());
    }

    public void TurboCollect()
    {
        currTurbo--;
    }
}
