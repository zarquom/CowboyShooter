using UnityEngine;

[CreateAssetMenu(fileName = "GameVariablesSO", menuName = "Scriptable Objects/GameVariablesSO")]
public class GameVariablesSO : ScriptableObject
{
    public WinCondition winCondition;
    public int pointsToWin;
    public int timeToWin;
    public float enemySpawnIncreaseRate;
    public float playerBulletSpeed;
    public float enemyBulletSpeed;
    public int enemyBasicChance;
    public int enemyFastChance;
    public int bossMaxLife;
    public int bossNormalLife;
    public float timeForBossAppearanceBossMode;
    public float timeForBossAppearanceOtherModes;
    public float powerupInterval;
    public MovementStrategy basicEnemyMovement;
    public MovementStrategy fastEnemyMovement;
    public MovementStrategy strongEnemyMovement;
    public MovementStrategy bossMovement;
}

public enum WinCondition
{
    Points,
    Time,
    BossDefeat
}
