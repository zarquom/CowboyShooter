using DG.Tweening;
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
    [SerializeField] private TMP_InputField userNameInput;
    [SerializeField] private CanvasGroup saveScoreCanvasGroup;
    [SerializeField] private Button playAgainButton;
    [SerializeField] private Button menuButton;
    [SerializeField] private Button saveButton;
    public event Action OnPlayAgainClicked;
    public event Action OnMenuClicked;

    private int finalPoints;

    public void Initialize(int initialPoints, int initialLives)
    {
        gameOverGameObject.gameObject.SetActive(false);
        UpdatePoints(initialPoints);
        UpdateLives(initialLives);
        ActivateSaveUserScore(false, true);
    }

    private void OnEnable()
    {
        playAgainButton.onClick.AddListener(OnPlayAgainButtonClicked);
        menuButton.onClick.AddListener(OnMenuButtonClicked);
        saveButton.onClick.AddListener(OnSaveButtonClicked);
    }

    private void OnDisable()
    {
        playAgainButton.onClick.RemoveListener(OnPlayAgainButtonClicked);
        menuButton.onClick.RemoveListener(OnMenuButtonClicked);
        saveButton.onClick.RemoveListener(OnSaveButtonClicked);
    }

    private void OnMenuButtonClicked()
    {
        OnMenuClicked?.Invoke();
    }

    private void OnPlayAgainButtonClicked()
    {
        OnPlayAgainClicked?.Invoke();
    }
    private void OnSaveButtonClicked()
    {
        ActivateSaveUserScore(false);
        ServiceLocator.GetService<ISaveService>().Save(userNameInput.text, finalPoints);
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

    public void ShowGameOverScreen(bool isWin, int finalPoints, bool showSaveScore)
    {
        this.finalPoints = finalPoints;
        gameOverGameObject.SetActive(true);
        gameoverText.text = isWin 
            ? $"You Win!\nFinal Points: {finalPoints}" 
            : $"Game Over!\nFinal Points: {finalPoints}";
        if (showSaveScore)
        {
            ActivateSaveUserScore(true);
        }

    }

    private void ActivateSaveUserScore(bool activate, bool instant = false)
    {
        saveScoreCanvasGroup.blocksRaycasts = activate;
        saveScoreCanvasGroup.interactable = activate;
        saveScoreCanvasGroup.DOFade(activate ? 1 : 0, instant ? 0f : 0.5f).SetUpdate(true);
    }
}
