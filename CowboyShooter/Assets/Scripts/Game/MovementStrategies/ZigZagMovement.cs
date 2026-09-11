using UnityEngine;

[CreateAssetMenu(menuName = "Movement/ZigZag")]
public class ZigZagMovement : MovementStrategy
{
    public float speed = 5f;
    public float horizontalSpeed = 4f;
    public float halfWidth = 2f;

    public override Vector2 GetVelocity(Rigidbody2D rb, ref MovementState s, float dt)
    {
        float offsetFromStart = rb.position.x - s.startPosition.x;
        // velocity.x sign flips once we've overshot the bound; store the sign in state so it's stable across frames
        if (Mathf.Abs(offsetFromStart) >= halfWidth)
            s.velocity.x = Mathf.Sign(offsetFromStart) * -horizontalSpeed;
        else if (s.velocity.x == 0f)
            s.velocity.x = horizontalSpeed;

        return new Vector2(s.velocity.x, -speed);
    }
}