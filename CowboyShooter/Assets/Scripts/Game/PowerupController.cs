using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class PowerupController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D powerupRigidbody;
    [SerializeField] private SpriteRenderer powerupImage;
    [SerializeField] private Sprite[] poweupImages;
    [SerializeField] private float moveSpeed = 2.4f;
    public event Action<PowerupController, bool> OnPowerupDeactivated;
    private PowerupType powerupType;
    public PowerupType PowerupType => powerupType;

    public void Initialize(PowerupType type)
    {
        powerupType = type;
        powerupImage.sprite = poweupImages[UnityEngine.Random.Range(0, poweupImages.Length)];
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
        powerupRigidbody.linearVelocity = Vector2.down * moveSpeed;
    }
    private void CheckBounds()
    {
        if (transform.position.y > 10f || transform.position.y < -10f || transform.position.x > 10f || transform.position.x < -10f)
        {
            DeactivatePowerup(false);
        }
    }

    public void DeactivatePowerup(bool obtained)
    {
        OnPowerupDeactivated?.Invoke(this, obtained);
        transform.position = Vector3.zero;
    }
}

public enum PowerupType
{
    Life,
    Damage,
    Bullet
}
