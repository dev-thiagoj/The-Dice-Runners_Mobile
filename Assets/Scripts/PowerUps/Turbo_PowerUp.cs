using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turbo_PowerUp : MonoBehaviour
{
    public float turboSpeed;
    public int turbosAmount = 3;
    public int currTurbo = 0;
    public float turboDuration = 2;
    bool _turboOn = false;

    PlayerController player;

    private void OnEnable()
    {
        ItemCollectableTurbo.onTurboCollect += TurboCollect;
    }

    private void OnDisable()
    {
        ItemCollectableTurbo.onTurboCollect -= TurboCollect;
    }

    protected void Awake()
    {
        player = GetComponent<PlayerController>();
    }

    protected void Start()
    {
        player._playerInputs.Gameplay.Turbo.performed += ctx => UseTurbo();
    }

    public void ChangeTurboOnValue()
    {
        _turboOn = !_turboOn;
    }

    public void UseTurbo()
    {
        if (player.characterController.isGrounded)
        {
            if (currTurbo < turbosAmount)
            {
                StartCoroutine(TurboCoroutine());
                currTurbo++;
                ItemManager.Instance.RemoveTurbo();
                return;
            }
            else if (currTurbo == 0)
            {
                ItemManager.Instance.WithoutTurboWarning();
                return;
            }
        }
    }

    public IEnumerator TurboCoroutine()
    {
        if (!_turboOn)
        {
            ChangeTurboOnValue();
            player.currRunSpeed = turboSpeed;
            SFXPool.Instance.Play(SFXType.USE_TURBO_06);
            yield return new WaitForSeconds(turboDuration);
        }

        player.currRunSpeed = player.runSpeed;
        StopCoroutine(TurboCoroutine());
        ChangeTurboOnValue();
    }

    public void TurboCollect()
    {
        currTurbo--;
    }
}
