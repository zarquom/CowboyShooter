using System;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D bulletRigidbody;
    [SerializeField] private SpriteRenderer bulletImage;
    [SerializeField] private Sprite[] bulletImages;
    public event Action<BulletController> OnBulletDeactivated;
    private float moveSpeed = 10f;
    private BulletType bulletType;
    private Vector2 targetDirection;
    private GameVariablesSO gameVariables;
    public void Initialize(BulletType type, GameVariablesSO gameVariables, Vector2 customDirection)
    {
        this.gameVariables = gameVariables;
        bulletType = type;
        targetDirection = customDirection;
        moveSpeed = bulletType == BulletType.Player ? gameVariables.playerBulletSpeed : gameVariables.enemyBulletSpeed;
        bulletImage.sprite = bulletImages[(int)bulletType];
        gameObject.layer = bulletType == BulletType.Player ? LayerMask.NameToLayer("BulletPlayer") : LayerMask.NameToLayer("BulletEnemy");
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
        if (transform.position.y > gameVariables.screenBoundY || transform.position.y < -gameVariables.screenBoundY || transform.position.x > gameVariables.screenBoundX || transform.position.x < -gameVariables.screenBoundX)
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
