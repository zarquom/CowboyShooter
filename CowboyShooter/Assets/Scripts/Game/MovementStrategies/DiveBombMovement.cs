using UnityEngine;

[CreateAssetMenu(menuName = "Movement/DiveBomb")]
public class DiveBombMovement : MovementStrategy
{
    public float hoverDuration = 1.5f;
    public float diveSpeed = 12f;

    public override Vector2 GetVelocity(Rigidbody2D rb, ref MovementState s, float dt)
    {
        s.elapsedTime += dt;
        return s.elapsedTime < hoverDuration ? Vector2.zero : Vector2.down * diveSpeed;
    }
}