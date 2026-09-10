using UnityEngine;

public class GamePreloader : MonoBehaviour
{
    private GameObject playerObj;
    void Awake()
    {
        playerObj = Instantiate(ServiceLocator.GetService<AssetLoaderManager>().GetAsset<GameObject>("Player"));
    }
}
