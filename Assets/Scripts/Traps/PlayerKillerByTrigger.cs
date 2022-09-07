using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKillerByTrigger : MonoBehaviour
{
    public AudioSource audioSource;

    [Range(0, 1)]
    public float sfxVolume;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            Actions.onDeadPlayer.Invoke();
            PlaySFX();
        }
    }

    // is used to play the specific trap kill sfx
    public void PlaySFX()
    {
        //SFXPool.Instance.Play(sfxType);
        if (audioSource != null)
        {
            audioSource.volume = sfxVolume;
            audioSource.Play();
        }

    }
}