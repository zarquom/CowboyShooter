using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuPreloader : MonoBehaviour
{
    private GameObject mainMenu;
    async void Awake()
    {
        mainMenu = Instantiate(ServiceLocator.GetService<IAssetLoader>().GetAsset<GameObject>("MainMenu"));
        await ServiceLocator.GetService<IAssetLoader>().PreloadLabelAsync("Game");
        Debug.Log("[MenuManager] Preloaded Game label");
    }
}
