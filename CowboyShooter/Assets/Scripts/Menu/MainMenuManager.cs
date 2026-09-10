using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MaineMenuManager : MonoBehaviour
{
    [SerializeField] private Button startButton;
    private bool GameAssetsLoaded => ServiceLocator.GetService<AssetLoaderManager>().IsLabelReady("Game");

    private void OnEnable()
    {
        startButton.onClick.AddListener(OnStartButtonClicked);
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
    }
}
