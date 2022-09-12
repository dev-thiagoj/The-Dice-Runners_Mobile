using System.Collections.Generic;
using UnityEngine;

public class StarsCalculate : MonoBehaviour
{
    [Header("References")]
    [SerializeField] PointsCalculator pointsCalculator;

    [Header("Final Stars")]
    public List<GameObject> fullStars;

    private void Awake()
    {
        if (pointsCalculator == null) pointsCalculator = GetComponent<PointsCalculator>();
    }

    public void UpdateUI()
    {
        Calculate();
    }

    void Calculate()
    {
        var totalScore = pointsCalculator.CalculateTotalScore();
        var maxPossibleScore = pointsCalculator.ActiveCollectablesCount();

        if (totalScore > (maxPossibleScore * 0.2f) && totalScore < (maxPossibleScore * 0.4f))
        {
            fullStars[0].SetActive(true);
        }
        else if (totalScore >= maxPossibleScore * 0.4f && totalScore < maxPossibleScore * 0.7f)
        {
            fullStars[0].SetActive(true);
            fullStars[1].SetActive(true);
        }
        else if (totalScore >= maxPossibleScore * 0.7f)
        {
            foreach (var star in fullStars)
            {
                star.SetActive(true);
            }
        }
    }
}
