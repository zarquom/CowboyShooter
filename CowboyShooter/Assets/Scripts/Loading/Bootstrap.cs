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
        ServiceLocator.RegisterService(new AssetLoaderManager());
        Debug.Log("[Bootstrap] Services initialized, loading next scene");
        SceneManager.LoadScene("Loading");
    }

    void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
            ServiceLocator.GetService<AssetLoaderManager>().ReleaseAllAssets();
            ServiceLocator.Clear();
        }
    }
}
