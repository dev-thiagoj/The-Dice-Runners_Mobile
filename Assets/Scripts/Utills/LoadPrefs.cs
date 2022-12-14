using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadPrefs : MonoBehaviour
{
    [Header("Volume Settings")]
    public AudioSource musicSource;
    public Slider volumeSlider;
    public TextMeshProUGUI sliderValue;

    public InstantiatePlayerHelper instantiatePlayer;

    private void Awake()
    {
        if (PlayerPrefs.HasKey("masterVolume"))
        {
            float localVolume = PlayerPrefs.GetFloat("masterVolume");

            sliderValue.text = localVolume.ToString("0.0");
            volumeSlider.value = localVolume;
            musicSource.volume = localVolume;
        }

        if (PlayerPrefs.HasKey("isRestart"))
        {
            int localValue = PlayerPrefs.GetInt("isRestart");

            GameManager.Instance.isRestart = localValue;
        }

        if (PlayerPrefs.HasKey("maxScore"))
        {
            var pointsCalculator = GameObject.Find("UIManager").GetComponent<PointsCalculator>();
            int localValue = PlayerPrefs.GetInt("maxScore");

            pointsCalculator.maxScore = localValue;
        }

        if (PlayerPrefs.HasKey("level"))
        {
            int localLevel = PlayerPrefs.GetInt("level");

            LevelManager.Instance.level = localLevel;
        }

        if (PlayerPrefs.HasKey("piecesNumber"))
        {
            int localValue = PlayerPrefs.GetInt("piecesNumber");

            LevelManager.Instance.numberOfPieces = localValue;
        }

        if (PlayerPrefs.HasKey("pieceIndex"))
        {
            int localIndex = PlayerPrefs.GetInt("pieceIndex");

            PiecesManager.Instance._index = localIndex;
        }

        if (PlayerPrefs.HasKey("currPlayerIndex"))
        {
            int index = PlayerPrefs.GetInt("currPlayerIndex");

            instantiatePlayer.characterIndex = index;
        }

        if (PlayerPrefs.HasKey("magneticsValue"))
        {
            int value = PlayerPrefs.GetInt("magneticsValue");
            var target = GameObject.Find("=== PLAYER ===").GetComponent<Magnetic_PowerUp>();

            target.magneticValue = value;
        }
    }
}
