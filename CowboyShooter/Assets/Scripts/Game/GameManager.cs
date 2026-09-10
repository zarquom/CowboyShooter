using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObjectsPoolManager gameObjectsPoolManager;
    private PlayerController playerController;

    private int playerPoints = 0;

    private float enemySpawnInterval = 2f;
    private float enemySpawnTimer = 0f;
    public void SetPlayer(PlayerController playerObj)
    {
        playerController = playerObj;
        playerController.OnAttack += HandlePlayerAttack;
    }

    private void HandlePlayerAttack()
    {
        BulletController bullet = gameObjectsPoolManager.GetBulletFromPool();
        bullet.transform.position = playerController.transform.position;
        bullet.OnBulletDeactivated += HandleBulletDeactivated;
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
        playerPoints++;
        OnDeactivateEnemy(enemy);
    }

    private void OnDeactivateEnemy(EnemyController enemy)
    {
        enemy.OnEnemyDeactivated -= HandleEnemyDeactivated;
        enemy.OnEnemyDestroyed -= HandleEnemyDestroyed;
        gameObjectsPoolManager.ReleaseEnemyToPool(enemy);
    }
}
