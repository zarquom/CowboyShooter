using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstrap : MonoBehaviour
{
    private static Bootstrap _instance;
    void Awake()
    {
        if (_instance != null) 
        {
            Destroy(gameObject); return; 
        }
        _instance = this;
        DontDestroyOnLoad(gameObject);

        //Load services and managers here
        ServiceLocator.RegisterService<IAssetLoader>(new AssetLoaderManager());
        ServiceLocator.RegisterService<ISaveService>(new PlayerPrefsSaveService());
        Debug.Log("[Bootstrap] Services initialized, loading next scene");
        SceneManager.LoadScene("Loading");
    }

    void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
            ServiceLocator.GetService<IAssetLoader>().ReleaseAllAssets();
            ServiceLocator.Clear();
        }
    }
}
