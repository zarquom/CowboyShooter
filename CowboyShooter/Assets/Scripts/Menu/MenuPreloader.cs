using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuPreloader : MonoBehaviour
{
    private GameObject mainMenu;
    private bool GameAssetsLoaded => ServiceLocator.GetService<AssetLoaderManager>().IsLabelReady("Game");
    async void Awake()
    {
        mainMenu = Instantiate(ServiceLocator.GetService<AssetLoaderManager>().GetAsset<GameObject>("MainMenu"));
        await ServiceLocator.GetService<AssetLoaderManager>().PreloadLabelAsync("Game");
        Debug.Log("[MenuManager] Preloaded Game label");
    }
}
