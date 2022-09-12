using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] LevelManager levelManager;
    [SerializeField] ItemManager itemManager;
    [SerializeField] PointsCalculator pointsCalculator;
    public TextMeshProUGUI scoreText = null;
    public TextMeshProUGUI diceText = null;
    public TextMeshProUGUI maxScoreText = null;

    [Header("UI Level")]
    public TextMeshProUGUI showUILevel;
    public int level;

    private void Awake()
    {
        if (levelManager == null) levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        if (itemManager == null) itemManager = GameObject.Find("ItemManager").GetComponent<ItemManager>();
        if (pointsCalculator == null) pointsCalculator = GetComponent<PointsCalculator>();
    }

    private void Start()
    {
        this.level = levelManager.level;
        showUILevel.text = "Level " + level;
    }

    public void UpdateUIScores()
    {
        scoreText.text = "Score: " + pointsCalculator.CalculateTotalScore().ToString("000");
        diceText.text = "Dices: " + itemManager.dice.ToString("000");

        if (pointsCalculator.IsMaxScoreReached())
        {
            maxScoreText.text = (pointsCalculator.maxScore).ToString();
            maxScoreText.color = Color.green;
            return;
        }
        maxScoreText.text = pointsCalculator.maxScore.ToString();
        maxScoreText.color = Color.yellow;
    }
}
