using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CrossfadeLevelLoader : MonoBehaviour
{
    public Animator crossfadeTransition;
    public float transitionTime = 1f;

    public void StartCrossfadeAnim(int i)
    {
        StartCoroutine(CrossfadeCoroutine(i));
    }

    IEnumerator CrossfadeCoroutine(int levelIndex)
    {
        crossfadeTransition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(levelIndex);
    }
}
