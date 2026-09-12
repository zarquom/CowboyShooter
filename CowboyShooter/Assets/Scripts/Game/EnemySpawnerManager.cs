using UnityEngine;

public class EnemySpawnerManager : MonoBehaviour
{
    private GameObjectsPoolManager gameObjectsPoolManager;
    private GameManager gameManager;
    private float enemySpawnInterval = 2f;
    private float enemySpawnTimer = 0f;
    private float enemySpawnDecreaseRate = 0f;

    public void Initialize(GameObjectsPoolManager poolManager, GameManager manager)
    {
        gameObjectsPoolManager = poolManager;
        gameManager = manager;
        enemySpawnDecreaseRate = gameManager.GameVariables.enemySpawnIncreaseRate;
    }

    public void SetSpawnInterval(float spawnInterval, float decreaseRate)
    {
        enemySpawnInterval = spawnInterval;
        enemySpawnDecreaseRate = decreaseRate;
    }
    public void ResetSpawnInterval(float spawnInterval)
    {
        enemySpawnInterval = spawnInterval;
    }
    private void Update()
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
            enemyController.OnEnemyDestroyed += gameManager.OnEnemyDestroyed;
            enemyController.OnAttack += gameManager.HandleEnemyAttack;
            enemySpawnTimer = 0f;
            if(enemySpawnInterval > 1f)
            {
                enemySpawnInterval -= enemySpawnDecreaseRate; // Decrease the spawn interval to increase difficulty
            }
        }
    }
    public void SpawnBoss(bool bigBoss = false)
    {
        BossController bossController = gameObjectsPoolManager.GetBossFromPool();
        bossController.transform.position = new Vector3(0f, 7f, 0f);
        bossController.Initialize(bigBoss, gameManager.GameVariables, gameManager.Player);
        bossController.OnEnemyDestroyed += gameManager.OnBossDestroyed;
        bossController.OnEnemyDeactivated += HandleBossDeactivated;
        bossController.OnAttack += gameManager.HandleEnemyAttack;
        bossController.OnHit += gameManager.HandleBulletHit;
    }
    private void HandleEnemyDeactivated(EnemyController enemy)
    {
        enemy.OnEnemyDeactivated -= HandleEnemyDeactivated;
        enemy.OnEnemyDestroyed -= gameManager.OnEnemyDestroyed;
        enemy.OnAttack -= gameManager.HandleEnemyAttack;
        gameObjectsPoolManager.ReleaseEnemyToPool(enemy);
    }
    private void HandleBossDeactivated(BossController boss)
    {
        boss.OnEnemyDeactivated -= HandleBossDeactivated;
        boss.OnEnemyDestroyed -= gameManager.OnBossDestroyed;
        boss.OnAttack -= gameManager.HandleEnemyAttack;
        gameObjectsPoolManager.ReleaseBossToPool(boss);
    }
}
