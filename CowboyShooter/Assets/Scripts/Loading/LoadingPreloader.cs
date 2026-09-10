using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingPreloader : MonoBehaviour
{
    async void Start()
    {
        await ServiceLocator.GetService<AssetLoaderManager>().PreloadLabelAsync("Menu");
        Debug.Log("[LoadingManager] Menu assets loaded, loading MainMenu");
        // After loading all menu assets, load the main menu scene
        SceneManager.LoadScene("MainMenu");
    }
}
