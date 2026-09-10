using System;
using UnityEngine;
public class EnemyController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Vector2 moveDirection = Vector2.down;
    [SerializeField] private Rigidbody2D playerRigidbody;
    public event Action<EnemyController> OnEnemyDeactivated;
    public event Action<EnemyController> OnEnemyDestroyed;

    private void Update()
    {
        CheckBounds();
    }
    void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        playerRigidbody.linearVelocity = moveDirection * moveSpeed;
    }

    private void CheckBounds()
    {
        if (transform.position.y < -10f)
        {
            Deactivate();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            OnEnemyDestroyed?.Invoke(this);
            //Add animation or effects here
            Deactivate();
            collision.gameObject.GetComponent<BulletController>().DeactivateBullet();
        }

    }

    private void Deactivate()
    {
        OnEnemyDeactivated?.Invoke(this);
    }
}
