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

        SceneManager.LoadScene("Loading");
    }
}
