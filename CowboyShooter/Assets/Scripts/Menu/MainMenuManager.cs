using DG.Tweening;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MaineMenuManager : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button highscoresButton;
    [SerializeField] private Button exitHighscoresButton;
    [SerializeField] private CanvasGroup highscoresScreen;
    [SerializeField] private Transform highscoresContent;
    private bool GameAssetsLoaded => ServiceLocator.GetService<IAssetLoader>().IsLabelReady("Game");
    void Start()
    {
        SetupHighscores();
    }
    private void OnEnable()
    {
        startButton.onClick.AddListener(OnStartButtonClicked);
        highscoresButton.onClick.AddListener(OnHighscoresButtonClicked);
        exitHighscoresButton.onClick.AddListener(OnExitHighscoresButtonClicked);
    }

    private void SetupHighscores()
    {
        ActivateHighScoresPanel(false, true);
        List<ScoreEntry> scores = ServiceLocator.GetService<ISaveService>().GetScores();
        for (int i = 0; i < scores.Count; i++)
        {
            HighscoreEntry entry = Instantiate(ServiceLocator.GetService<IAssetLoader>().GetAsset<GameObject>("HighscoreEntry"), highscoresContent).GetComponent<HighscoreEntry>();
            entry.SetupEntry(scores[i].UserName, scores[i].Score, i);
        }
    }

    private void OnHighscoresButtonClicked()
    {
        ActivateHighScoresPanel(true);
    }
    private void OnExitHighscoresButtonClicked()
    {
        ActivateHighScoresPanel(false);
    }

    private void ActivateHighScoresPanel(bool activate, bool instant = false)
    {
        highscoresScreen.blocksRaycasts = activate;
        highscoresScreen.interactable = activate;
        highscoresScreen.DOFade(activate ? 1 : 0, instant ? 0 : 0.5f).SetUpdate(true);
    }
    private void OnStartButtonClicked()
    {
        StartCoroutine(LoadGameScene());
    }

    private IEnumerator LoadGameScene()
    {
        while (!GameAssetsLoaded)
        {
            yield return new WaitForSeconds(1f);
        }
        SceneManager.LoadScene("Game");
    }

    private void OnDisable()
    {
        startButton.onClick.RemoveListener(OnStartButtonClicked);
        highscoresButton.onClick.RemoveListener(OnHighscoresButtonClicked);
        exitHighscoresButton.onClick.RemoveListener(OnExitHighscoresButtonClicked);
    }
}
