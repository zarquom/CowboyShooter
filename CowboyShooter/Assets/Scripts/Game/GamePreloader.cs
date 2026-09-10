using UnityEngine;

public class GamePreloader : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    private GameObject playerObj;
    void Awake()
    {
        playerObj = Instantiate(ServiceLocator.GetService<AssetLoaderManager>().GetAsset<GameObject>("Player"));
        gameManager.SetPlayer(playerObj);
    }
}
