using UnityEngine;
using Singleton;

public class PointsCalculator : Singleton<PointsCalculator>
{
    [Header("Points")]
    public int finalScore;
    public int turboScore;
    public int maxScore;

    int activeDices;
    int activeTurbos;
    int maxPossibleScore;
    int totalScore;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        Invoke(nameof(ActiveCollectablesCount), 2);
    }

    public int ActiveCollectablesCount()
    {
        int turbos = GameObject.Find("=== PLAYER ===").GetComponent<Turbo_PowerUp>().turbosAmount;

        activeDices = FindObjectsOfType(typeof(ItemCollectableDice)).Length;
        activeTurbos = FindObjectsOfType(typeof(ItemCollectableTurbo)).Length;

        maxPossibleScore = activeDices * (activeTurbos + turbos);
        return maxPossibleScore;
    }

    public int TurnTurboInPoints()
    {
        turboScore = ItemManager.Instance.turbo;

        if (turboScore == 0) turboScore = 1;

        return turboScore;
    }

    public int CalculateTotalScore()
    {
        totalScore = ItemManager.Instance.dice * TurnTurboInPoints();
        return totalScore;
    }

    public bool IsMaxScoreReached()
    {
        var totalScore = CalculateTotalScore();

        if (totalScore > maxScore)
        {
            maxScore = totalScore;
            PlayerPrefs.SetInt("maxScore", maxScore);
            return true;
        }
        return false;
    }
}
