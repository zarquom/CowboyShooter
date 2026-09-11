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
    public MovementStrategy basicEnemyMovement;
    public MovementStrategy fastEnemyMovement;
    public MovementStrategy strongEnemyMovement;
}

public enum WinCondition
{
    Points,
    Time,
    BossDefeat
}
