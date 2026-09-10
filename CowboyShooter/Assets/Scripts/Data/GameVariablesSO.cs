using UnityEngine;

[CreateAssetMenu(fileName = "GameVariablesSO", menuName = "Scriptable Objects/GameVariablesSO")]
public class GameVariablesSO : ScriptableObject
{
    public WinCondition winCondition;
    public int pointsToWin;
    public int timeToWin;
}

public enum WinCondition
{
    Points,
    Time,
    BossDefeat
}
