using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StarsCalculate : MonoBehaviour
{
    /*[Header("References")]
    public TextMeshProUGUI scoreText = null;
    public TextMeshProUGUI diceText = null;
    public TextMeshProUGUI maxScoreText = null;*/

    [Header("Final Stars")]
    public List<GameObject> fullStars;

    public void UpdateUI()
    {
        Calculate();
        //scoreText.text = "Score: " + PointsCalculator.Instance.Calculate().ToString("000");
        //diceText.text = "Dices: " + ItemManager.Instance.dice.ToString("000");
    }

    void Calculate()
    {
        var totalScore = PointsCalculator.Instance.CalculateTotalScore();
        var maxPossibleScore = PointsCalculator.Instance.ActiveCollectablesCount();

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
