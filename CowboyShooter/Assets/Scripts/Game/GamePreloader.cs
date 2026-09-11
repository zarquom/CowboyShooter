using UnityEngine;

public class GamePreloader : MonoBehaviour
{
    private GameManager gameManager;
    private PlayerController playerObj;
    private GameUIManager gameUIManager;
    void Awake()
    {
        gameManager = Instantiate(ServiceLocator.GetService<IAssetLoader>().GetAsset<GameObject>("GameManager")).GetComponent<GameManager>();
        playerObj = Instantiate(ServiceLocator.GetService<IAssetLoader>().GetAsset<GameObject>("Player")).GetComponent<PlayerController>()        ;
        gameUIManager = Instantiate(ServiceLocator.GetService<IAssetLoader>().GetAsset<GameObject>("GameUI")).GetComponent<GameUIManager>();
        Instantiate(ServiceLocator.GetService<IAssetLoader>().GetAsset<GameObject>("Background"));
        gameManager.SetPlayer(playerObj);
        gameManager.SetGameUI(gameUIManager);
    }
}
