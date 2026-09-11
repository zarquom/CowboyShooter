using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawnerManager : MonoBehaviour
{
    private GameObjectsPoolManager gameObjectsPoolManager;
    private GameManager gameManager;
    private float enemySpawnInterval = 2f;
    private float enemySpawnTimer = 0f;

    public void Initialize(GameObjectsPoolManager poolManager, GameManager manager)
    {
        gameObjectsPoolManager = poolManager;
        gameManager = manager;
    }
    // Update is called once per frame
    void Update()
    {
        if (!gameManager.GameRunning) return;
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
            EnemyType enemyType = EnemyType.Basic; // Default value
            int randomValue = UnityEngine.Random.Range(0, 100);
            if (randomValue < 60) enemyType = EnemyType.Basic;
            else if (randomValue < 90) enemyType = EnemyType.Fast;
            else enemyType = EnemyType.Strong;
            enemyController.Initialize(enemyType);
            enemyController.OnEnemyDeactivated += HandleEnemyDeactivated;
            enemyController.OnEnemyDestroyed += HandleEnemyDestroyed;
            enemySpawnTimer = 0f;
        }
    }
    private void HandleEnemyDestroyed(EnemyController enemy)
    {
        gameManager.OnEnemyDestroyed(enemy);
        OnDeactivateEnemy(enemy);
    }
    private void HandleEnemyDeactivated(EnemyController enemy)
    {
        OnDeactivateEnemy(enemy);
    }
    private void OnDeactivateEnemy(EnemyController enemy)
    {
        enemy.OnEnemyDeactivated -= HandleEnemyDeactivated;
        enemy.OnEnemyDestroyed -= HandleEnemyDestroyed;
        gameObjectsPoolManager.ReleaseEnemyToPool(enemy);
    }
}
