using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI pointsText;
    [SerializeField] private TextMeshProUGUI livesText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private GameObject gameOverGameObject;
    [SerializeField] private TextMeshProUGUI gameoverText;
    [SerializeField] private Button playAgainButton;
    [SerializeField] private Button menuButton;
    public event Action OnPlayAgainClicked;
    public event Action OnMenuClicked;

    public void Initialize(int initialPoints, int initialLives)
    {
        gameOverGameObject.gameObject.SetActive(false);
        UpdatePoints(initialPoints);
        UpdateLives(initialLives);
    }

    private void OnEnable()
    {
        playAgainButton.onClick.AddListener(OnPlayAgainButtonClicked);
        menuButton.onClick.AddListener(OnMenuButtonClicked);

    }

    private void OnDisable()
    {
        playAgainButton.onClick.RemoveListener(OnPlayAgainButtonClicked);
        menuButton.onClick.RemoveListener(OnMenuButtonClicked);
    }

    private void OnMenuButtonClicked()
    {
        OnMenuClicked?.Invoke();
    }

    private void OnPlayAgainButtonClicked()
    {
        OnPlayAgainClicked?.Invoke();
    }

    public void UpdatePoints(int points)
    {
        pointsText.text = $"Points: {points}";
    }

    public void UpdateLives(int lives)
    {
        livesText.text = $"Lives: {lives}";
    }

    public void UpdateTimer(float timeRemaining)
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(timeRemaining);
        timerText.text = $"Time: {timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";
    }

    public void ShowGameOverScreen(bool isWin, int finalPoints)
    {
        gameOverGameObject.SetActive(true);
        gameoverText.text = isWin 
            ? $"You Win!\nFinal Points: {finalPoints}" 
            : $"Game Over!\nFinal Points: {finalPoints}";
    }
}
