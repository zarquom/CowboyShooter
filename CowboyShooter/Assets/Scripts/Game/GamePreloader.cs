using UnityEngine;

public class GamePreloader : MonoBehaviour
{
    private GameManager gameManager;
    private PlayerController playerObj;
    void Awake()
    {
        gameManager = Instantiate(ServiceLocator.GetService<AssetLoaderManager>().GetAsset<GameObject>("GameManager")).GetComponent<GameManager>();
        playerObj = Instantiate(ServiceLocator.GetService<AssetLoaderManager>().GetAsset<GameObject>("Player")).GetComponent<PlayerController>()        ;
        gameManager.SetPlayer(playerObj);
    }
}
