using UnityEngine;

public abstract class MovementStrategy : ScriptableObject
{
    // Returns the desired velocity this physics step — Rigidbody2D applies it, never touches transform directly
    public abstract Vector2 GetVelocity(Rigidbody2D rb, ref MovementState state, float fixedDeltaTime);
}

public struct MovementState
{
    public Vector3 startPosition;
    public float elapsedTime;
    public Vector3 velocity;
}