using UnityEngine;

[CreateAssetMenu(fileName = "GameVariablesSO", menuName = "Scriptable Objects/GameVariablesSO")]
public class GameVariablesSO : ScriptableObject
{
    [Header("Win Conditions")]
    public WinCondition winCondition;
    public int pointsToWin;
    public int timeToWin;

    [Header("Movement Strategies")]
    public MovementStrategy basicEnemyMovement;
    public MovementStrategy fastEnemyMovement;
    public MovementStrategy strongEnemyMovement;
    public MovementStrategy bossMovement;
    public MovementStrategy bossMovementSecondary;

    [Header("Player")]
    public float playerBulletSpeed;
    public float playerMaxLife = 100f;
    public float playerEnemyContactDamage = 1f;
    public float playerBulletDamage = 15f;
    public float playerLifePowerupHealAmount = 50f;
    public float playerDamagePowerupAmount = 20f;
    public float bulletPowerupDuration = 5f;
    public float bulletSpreadOffset = 0.1f;

    [Header("Scoring")]
    public int pointsBasicEnemy = 1;
    public int pointsStrongEnemy = 5;
    public int pointsBoss = 50;

    [Header("Enemy")]
    public float enemySpawnIncreaseRate;
    public float enemyBulletSpeed;
    public int enemyBasicChance;
    public int enemyFastChance;
    public float enemyBasicScale = 2f;
    public float enemyStrongScale = 2.5f;
    public int enemyBasicLifeHits = 1;
    public int enemyStrongLifeHits = 3;
    public float enemyAttackInitialDelay = 2f;
    public float enemyAttackIntervalMin = 3f;
    public float enemyAttackIntervalMax = 6f;
    public float enemyDeathAnimationDuration = 0.5f;
    public float initialEnemySpawnInterval = 2f;
    public float minEnemySpawnInterval = 1f;
    public float enemySpawnMinX = -8f;
    public float enemySpawnMaxX = 8f;
    public float enemySpawnPositionY = 10f;
    public float enemyDespawnMinY = -10f;
    public float enemyDespawnMaxX = 15f;

    [Header("Boss")]
    public int bossMaxLife;
    public int bossNormalLife;
    public float timeForBossAppearanceBossMode;
    public float timeForBossAppearanceOtherModes;
    public float bossAttackInitialDelay = 2f;
    public float bossAttackIntervalMin = 2f;
    public float bossAttackIntervalMax = 4f;
    public float bossDeathAnimationDuration = 0.8f;
    public float bossSpawnPositionY = 7f;
    public float bossActiveSpawnInterval = 6f;
    public float postBossDefeatSpawnInterval = 2f;
    public float bossResetMovementPositionY = -2f;
    public int bossAttacksCounterForChangeMovement = 3;

    [Header("Spawn Wobble (Boss / Orbit enemies)")]
    public float topStartPositionXRange = 1f;
    public float topStartPositionY = 3f;

    [Header("Projectiles & Powerups")]
    public float powerupInterval;
    public float screenBoundX = 10f;
    public float screenBoundY = 10f;
    public float powerupSpawnMinX = -6f;
    public float powerupSpawnMaxX = 6f;
    public float powerupSpawnPositionY = 6f;
}

public enum WinCondition
{
    Points,
    Time,
    BossDefeat
}
