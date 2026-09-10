using System;
using TMPro;
using UnityEngine;

public class GameUIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI pointsText;
    [SerializeField] private TextMeshProUGUI livesText;

    public void Initialize(int initialPoints, int initialLives)
    {
        UpdatePoints(initialPoints);
        UpdateLives(initialLives);
    }

    public void UpdatePoints(int points)
    {
        pointsText.text = $"Points: {points}";
    }

    public void UpdateLives(int lives)
    {
        livesText.text = $"Lives: {lives}";
    }
}
