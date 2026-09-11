using UnityEngine;

[CreateAssetMenu(menuName = "Movement/Orbit")]
public class OrbitMovement : MovementStrategy
{
    public float radius = 3f;
    public float angularSpeedDegreesPerSecond = 90f;

    public override Vector2 GetVelocity(Rigidbody2D rb, ref MovementState s, float dt)
    {
        s.elapsedTime += dt;
        float angle = s.elapsedTime * angularSpeedDegreesPerSecond * Mathf.Deg2Rad;

        Vector2 targetPos = (Vector2)s.startPosition + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
        return ((targetPos - rb.position) / dt).normalized;
    }
}