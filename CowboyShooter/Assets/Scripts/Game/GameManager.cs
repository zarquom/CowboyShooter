using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObjectsPoolManager gameObjectsPoolManager;
    private PlayerController playerController;
    private GameUIManager gameUIManager;

    private int playerPoints = 0;
    private int playerLives = 3;

    private float enemySpawnInterval = 2f;
    private float enemySpawnTimer = 0f;
    public void SetPlayer(PlayerController playerObj)
    {
        playerController = playerObj;
        playerController.OnAttack += HandlePlayerAttack;
        playerController.OnDeath += HandlePlayerDeath;
    }
    public void SetGameUI(GameUIManager uiManager)
    {
        gameUIManager = uiManager;
        gameUIManager.Initialize(playerPoints, playerLives);
    }

    private void HandlePlayerAttack()
    {
        BulletController bullet = gameObjectsPoolManager.GetBulletFromPool();
        bullet.transform.position = playerController.transform.position;
        bullet.OnBulletDeactivated += HandleBulletDeactivated;
    }
    private void HandlePlayerDeath()
    {
        playerLives--;
        gameUIManager.UpdateLives(playerLives);
        if (playerLives <= 0)
        {
            // Handle game over
        }
    }

    private void HandleBulletDeactivated(BulletController bullet)
    {
        bullet.OnBulletDeactivated -= HandleBulletDeactivated;
        gameObjectsPoolManager.ReleaseBulletToPool(bullet);
    }

    private void Update()
    {
        EnemyCreation();
    }

    private void EnemyCreation()
    {
        enemySpawnTimer += Time.deltaTime;
        if (enemySpawnTimer >= enemySpawnInterval)
        {
            EnemyController enemyController = gameObjectsPoolManager.GetEnemyFromPool();
            float randomX = UnityEngine.Random.Range(-8f, 8f);
            enemyController.transform.position = new Vector3(randomX, 10f, 0f);
            enemyController.OnEnemyDeactivated += HandleEnemyDeactivated;
            enemyController.OnEnemyDestroyed += HandleEnemyDestroyed;
            enemySpawnTimer = 0f;
        }
    }

    private void HandleEnemyDeactivated(EnemyController enemy)
    {
        OnDeactivateEnemy(enemy);
    }

    private void HandleEnemyDestroyed(EnemyController enemy)
    {
        Debug.Log("Enemy destroyed");
        playerPoints++;
        gameUIManager.UpdatePoints(playerPoints);
        OnDeactivateEnemy(enemy);
    }

    private void OnDeactivateEnemy(EnemyController enemy)
    {
        enemy.OnEnemyDeactivated -= HandleEnemyDeactivated;
        enemy.OnEnemyDestroyed -= HandleEnemyDestroyed;
        gameObjectsPoolManager.ReleaseEnemyToPool(enemy);
    }
}
