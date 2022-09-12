using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    /*[Header("References")]
    public GameObject mainMenu;
    public GameObject uiContainer;
    public GameObject virtualJoysticks;*/
    public TextMeshProUGUI scoreText = null;
    public TextMeshProUGUI diceText = null;
    public TextMeshProUGUI maxScoreText = null;

    /*[Header("Buttons Animation")]
    public GameObject btnContainer;
    public Ease ease;
    public float timeBtnAnim;

    [Header("Level Complete")]
    public GameObject levelCompleteScreen;

    [Header("GameOver Screen")]
    public GameObject gameOverScreen;

    [Header("Pause Game")]
    public GameObject pauseScreen;*/

    [Header("UI Level")]
    public TextMeshProUGUI showUILevel;
    public int level;

    /*[Header("Mini Map")]
    public Transform miniMap;*/

    private void Start()
    {
        this.level = LevelManager.Instance.level;
        showUILevel.text = "Level " + level;
    }

    public void UpdateUIScores()
    {
        scoreText.text = "Score: " + PointsCalculator.Instance.CalculateTotalScore().ToString("000");
        diceText.text = "Dices: " + ItemManager.Instance.dice.ToString("000");

        if (PointsCalculator.Instance.IsMaxScoreReached())
        {
            maxScoreText.text = (PointsCalculator.Instance.maxScore).ToString();
            maxScoreText.color = Color.green;
            return;
        }
        maxScoreText.text = PointsCalculator.Instance.maxScore.ToString();
        maxScoreText.color = Color.yellow;
    }
}
