using UnityEngine;

public class GamePreloader : MonoBehaviour
{
    private GameManager gameManager;
    private PlayerController playerObj;
    private GameUIManager gameUIManager;
    void Awake()
    {
        gameManager = Instantiate(ServiceLocator.GetService<AssetLoaderManager>().GetAsset<GameObject>("GameManager")).GetComponent<GameManager>();
        playerObj = Instantiate(ServiceLocator.GetService<AssetLoaderManager>().GetAsset<GameObject>("Player")).GetComponent<PlayerController>()        ;
        gameUIManager = Instantiate(ServiceLocator.GetService<AssetLoaderManager>().GetAsset<GameObject>("GameUI")).GetComponent<GameUIManager>();
        gameManager.SetPlayer(playerObj);
        gameManager.SetGameUI(gameUIManager);
    }
}
