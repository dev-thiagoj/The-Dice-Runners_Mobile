using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneHelper : MonoBehaviour
{
    public bool loadAuto = false;
    public int sceneToLoad;

    private void Start()
    {
        if (loadAuto == true) Invoke(nameof(LoadScene), 15);
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
