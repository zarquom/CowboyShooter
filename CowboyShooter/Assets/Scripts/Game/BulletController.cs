using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class BulletController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D playerRigidbody;
    public event Action<BulletController> OnBulletDeactivated;
    private float moveSpeed = 10f;
    void Update()
    {
        CheckBounds();
    }
    void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        playerRigidbody.linearVelocity = Vector2.up * moveSpeed;
    }

    private void CheckBounds()
    {
        if (transform.position.y > 10f)
        {
            DeactivateBullet();
        }
    }

    public void DeactivateBullet()
    {
        OnBulletDeactivated?.Invoke(this);
        transform.position = Vector3.zero;
    }
}
