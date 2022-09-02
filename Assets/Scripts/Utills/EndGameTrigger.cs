using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            GameManager.Instance.checkedEndLine = true;
            PlayerController.Instance.playerAnimation.SetTriggerByString("Idle");
            PlayerController.Instance.rotationLook.canLook = true;
            GameManager.Instance.LevelComplete();
        }
    }
}
