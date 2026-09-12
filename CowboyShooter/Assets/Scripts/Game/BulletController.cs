using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class BulletController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D bulletRigidbody;
    [SerializeField] private SpriteRenderer bulletImage;
    [SerializeField] private Sprite[] bulletImages;
    public event Action<BulletController> OnBulletDeactivated;
    private float moveSpeed = 10f;
    private BulletType bulletType;
    private Vector2 targetDirection;
    public void Initialize(BulletType type, GameVariablesSO gameVariables, Vector2 customDirection)
    {
        bulletType = type;
        targetDirection = customDirection;
        moveSpeed = bulletType == BulletType.Player ? gameVariables.playerBulletSpeed : gameVariables.enemyBulletSpeed;
        bulletImage.sprite = bulletImages[(int)bulletType];
        gameObject.layer = bulletType == BulletType.Player ? 8 : 9;
        transform.localScale = bulletType == BulletType.Player ? new Vector3(1f, 1f, 1f) : new Vector3(1f, -1f, 1f);
    }
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
        bulletRigidbody.linearVelocity = targetDirection * moveSpeed;
    }

    private void CheckBounds()
    {
        if (transform.position.y > 10f || transform.position.y < -10f || transform.position.x > 10f || transform.position.x < -10f)
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

public enum BulletType
{
    Player,
    Enemy
}
