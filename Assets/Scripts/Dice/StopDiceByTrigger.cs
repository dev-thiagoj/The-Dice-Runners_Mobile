using System;
using UnityEngine;

public class StopDiceByTrigger : MonoBehaviour
{
    public RollDice masterDice;
    public static Action onDiceTriggered; 

    private void Start()
    {
        masterDice = GameObject.Find("=== DICE ===").GetComponent<RollDice>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Dice"))
        {
            //masterDice.canMove = false;
            //masterDice.StopVFX();
            //masterDice.audioSource.Stop();
            onDiceTriggered.Invoke();
        }
    }
}
