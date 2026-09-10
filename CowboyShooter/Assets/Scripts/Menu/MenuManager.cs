using UnityEngine;

public class MenuManager : MonoBehaviour
{
    private GameObject mainMenu;
    void Awake()
    {
        mainMenu = Instantiate(ServiceLocator.GetService<AssetLoaderManager>().GetAsset<GameObject>("MainMenu"));
    }
}
