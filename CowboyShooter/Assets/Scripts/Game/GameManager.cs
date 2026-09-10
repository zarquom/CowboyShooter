using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObjectsPoolManager gameObjectsPoolManager;
    private PlayerController playerController;
    public void SetPlayer(GameObject playerObj)
    {
        playerController = playerObj.GetComponent<PlayerController>();
        playerController.OnAttack += HandlePlayerAttack;
    }

    private void HandlePlayerAttack()
    {
        BulletController bullet = gameObjectsPoolManager.GetBulletFromPool();
        bullet.transform.position = playerController.transform.position;
    }
}
