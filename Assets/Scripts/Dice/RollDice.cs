using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Singleton;
using System;

public class RollDice : MonoBehaviour
{
    public Rigidbody rigidbody;
    public Transform diceToRoll;
    public AudioSource audioSource;
    public ParticleSystem particleSystem;

    [Range(4, 8)]
    public float speedRoll = 3;
    public float moveSpeed = 5;
    public float startSFXDelay = 3;
    public bool canMove;

    private void OnEnable()
    {
        Actions.onGameStarted += InvokeStartRoll;
        StopDiceByTrigger.onDiceTriggered += StopDiceAtTheEnd;
    }

    private void OnDisable()
    {
        Actions.onGameStarted -= InvokeStartRoll;
        StopDiceByTrigger.onDiceTriggered -= StopDiceAtTheEnd;
    }

    private void OnValidate()
    {
        if (particleSystem == null) particleSystem = GetComponentInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            rigidbody.transform.position += Vector3.forward * moveSpeed * Time.deltaTime;
            
            diceToRoll.transform.Rotate(0.0f, (-speedRoll * Time.deltaTime * 10), 0.0f);
        }
    }

    public void InvokeStartRoll()
    {
        CallDiceSFX();
        Invoke(nameof(StartRoll), 3);
    }

    public void StartRoll()
    {
        canMove = true;
    }

    public void DestroyDice()
    {
        Destroy(gameObject, 5);
    }

    public void CallDiceSFX()
    {
        Invoke(nameof(PlaySFX), startSFXDelay);
    }

    public void PlaySFX()
    {
        audioSource.Play();
    }

    void StopDiceAtTheEnd()
    {
        canMove = false;
        particleSystem.Stop();
        audioSource.Stop();
    }
}
