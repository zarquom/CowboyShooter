using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuPreloader : MonoBehaviour
{
    private MainMenuManager mainMenu;
    private AudioManager mainMenuAudio;
    async void Awake()
    {
        mainMenu = Instantiate(ServiceLocator.GetService<IAssetLoader>().GetAsset<GameObject>("MainMenu")).GetComponent<MainMenuManager>();
        mainMenuAudio = Instantiate(ServiceLocator.GetService<IAssetLoader>().GetAsset<GameObject>("MenuAudio")).GetComponent<AudioManager>();
        mainMenu.SetAudioManager(mainMenuAudio);
        await ServiceLocator.GetService<IAssetLoader>().PreloadLabelAsync("Game");
        Debug.Log("[MenuManager] Preloaded Game label");
    }
}
