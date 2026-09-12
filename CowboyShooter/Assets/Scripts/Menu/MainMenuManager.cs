using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button highscoresButton;
    [SerializeField] private Button exitHighscoresButton;
    [SerializeField] private CanvasGroup highscoresScreen;
    [SerializeField] private Transform highscoresContent;
    [SerializeField] private Button volumeButton;
    [SerializeField] private Sprite[] volumeButtonSprites;

    private AudioManager audioManager;
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
        volumeButton.onClick.AddListener(OnVolumeButtonClicked);
    }

    private void OnVolumeButtonClicked()
    {
        float currentVolume = ServiceLocator.GetService<ISaveService>().GetVolume();
        if (currentVolume > 0)
        {
            audioManager.SetVolume(0f);
            ServiceLocator.GetService<ISaveService>().SetVolume(0f);
            volumeButton.image.sprite = volumeButtonSprites[1];
        } else
        {
            audioManager.SetVolume(1f);
            ServiceLocator.GetService<ISaveService>().SetVolume(1f);
            volumeButton.image.sprite = volumeButtonSprites[0];
        }
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
        audioManager.PlaySound("Button2");
        ActivateHighScoresPanel(true);
    }
    private void OnExitHighscoresButtonClicked()
    {
        audioManager.PlaySound("Button1");
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
        audioManager.PlaySound("Button2");
        StartCoroutine(LoadGameScene());
    }

    private IEnumerator LoadGameScene()
    {
        while (!GameAssetsLoaded)
        {
            yield return new WaitForSeconds(1f);
        }
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Game");
    }

    private void OnDisable()
    {
        startButton.onClick.RemoveListener(OnStartButtonClicked);
        highscoresButton.onClick.RemoveListener(OnHighscoresButtonClicked);
        exitHighscoresButton.onClick.RemoveListener(OnExitHighscoresButtonClicked);
        volumeButton.onClick.RemoveListener(OnVolumeButtonClicked);
    }

    public void SetAudioManager(AudioManager mainMenuAudio)
    {
        audioManager = mainMenuAudio;
        audioManager.SetVolume(ServiceLocator.GetService<ISaveService>().GetVolume());
    }
}
