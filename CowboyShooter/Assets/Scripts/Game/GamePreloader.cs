using UnityEngine;

public class GamePreloader : MonoBehaviour
{
    private GameManager gameManager;
    private PlayerController playerObj;
    private GameUIManager gameUIManager;
    private AudioManager audioManager;
    void Awake()
    {
        gameManager = Instantiate(ServiceLocator.GetService<IAssetLoader>().GetAsset<GameObject>("GameManager")).GetComponent<GameManager>();
        playerObj = Instantiate(ServiceLocator.GetService<IAssetLoader>().GetAsset<GameObject>("Player")).GetComponent<PlayerController>()        ;
        gameUIManager = Instantiate(ServiceLocator.GetService<IAssetLoader>().GetAsset<GameObject>("GameUI")).GetComponent<GameUIManager>();
        audioManager = Instantiate(ServiceLocator.GetService<IAssetLoader>().GetAsset<GameObject>("GameAudio")).GetComponent<AudioManager>();
        Instantiate(ServiceLocator.GetService<IAssetLoader>().GetAsset<GameObject>("Background"));
        gameManager.SetPlayer(playerObj);
        gameManager.SetAudioManager(audioManager);
        gameManager.SetGameUI(gameUIManager);
    }
}
