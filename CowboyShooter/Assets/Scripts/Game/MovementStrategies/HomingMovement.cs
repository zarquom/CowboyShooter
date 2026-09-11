using UnityEngine;

[CreateAssetMenu(menuName = "Movement/Homing")]
public class HomingMovement : MovementStrategy
{
    public float speed = 4f;
    public float turnRateDegreesPerSecond = 120f;
    public Transform target; // assign the player, or have Enemy inject it at spawn

    public override Vector2 GetVelocity(Rigidbody2D rb, ref MovementState s, float dt)
    {
        if (target == null) return Vector2.down * speed;
        if(rb.position.y < -3f) return Vector2.down * speed; // don't chase player if we're down the screen
        Vector2 toTarget = (Vector2)target.position - rb.position;
        Vector2 desiredDir = toTarget.normalized;

        Vector2 currentDir = s.velocity.sqrMagnitude > 0.01f ? s.velocity.normalized : Vector2.down;
        float maxRadians = turnRateDegreesPerSecond * Mathf.Deg2Rad * dt;
        Vector2 newDir = Vector3.RotateTowards(currentDir, desiredDir, maxRadians, 0f);

        s.velocity = newDir * speed;
        return s.velocity;
    }
}