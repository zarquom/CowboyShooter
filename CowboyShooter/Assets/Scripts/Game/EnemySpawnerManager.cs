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
            if (randomValue < gameManager.GameVariables.enemyBasicChance) enemyType = EnemyType.Basic;
            else if (randomValue < gameManager.GameVariables.enemyBasicChance + gameManager.GameVariables.enemyFastChance) enemyType = EnemyType.Fast;
            else enemyType = EnemyType.Strong;
            enemyController.Initialize(enemyType, gameManager.GameVariables, gameManager.Player);
            enemyController.OnEnemyDeactivated += HandleEnemyDeactivated;
            enemyController.OnEnemyDestroyed += HandleEnemyDestroyed;
            enemyController.OnAttack += gameManager.HandleEnemyAttack;
            enemySpawnTimer = 0f;
            if(enemySpawnInterval > 1f)
            {
                enemySpawnInterval -= gameManager.GameVariables.enemySpawnIncreaseRate; // Decrease the spawn interval to increase difficulty
            }
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
        enemy.OnAttack -= gameManager.HandleEnemyAttack;
        gameObjectsPoolManager.ReleaseEnemyToPool(enemy);
    }
}
