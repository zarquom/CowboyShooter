using System;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Rigidbody2D playerRigidbody;
    [SerializeField] private BoxCollider2D playerCollider;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private LifeBarObject healthObj;
    [SerializeField] private GameObject healthBar;
    public event Action<bool> OnAttack;
    public event Action OnDeath;
    public event Action OnHit;
    private InputSystem_Actions inputActions;

    private float currentLife = 100f;
    private float bulletPowerup = 0f;
    private bool gameRunning = true;
    void Start()
    {
        inputActions = new InputSystem_Actions();
        inputActions.Enable();
        inputActions.Player.Attack.performed += OnAttackPerformed;
        bulletPowerup = 0f;
    }

    void OnDestroy()
    {
        inputActions.Player.Attack.performed -= OnAttackPerformed;
        inputActions.Disable();
    }

    private void OnAttackPerformed(InputAction.CallbackContext context)
    {
        OnAttack?.Invoke(bulletPowerup > 0f);
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }
    private void Update()
    {
        if(gameRunning && bulletPowerup > 0f)
        {
            bulletPowerup -= Time.deltaTime;
        }
    }
    private void HandleMovement()
    {
        Vector2 moveInput = inputActions.Player.Move.ReadValue<Vector2>();
        playerRigidbody.linearVelocity = moveInput * moveSpeed;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(!gameRunning) return;
        if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(1f);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            TakeDamage(15f);
            OnHit?.Invoke();
            collision.gameObject.GetComponent<BulletController>().DeactivateBullet();
            Instantiate(ServiceLocator.GetService<IAssetLoader>().GetAsset<GameObject>("BulletSparkles"), transform.position, transform.rotation);
        }
        if (collision.gameObject.CompareTag("Powerup"))
        {
            PowerupController powerupController = collision.gameObject.GetComponent<PowerupController>();
            PowerupEffect(powerupController.PowerupType);
            powerupController.DeactivatePowerup(true);
            Instantiate(ServiceLocator.GetService<IAssetLoader>().GetAsset<GameObject>("PowerupSparkles"), transform.position, transform.rotation);
        }
    }

    private void PowerupEffect(PowerupType powerupType)
    {
        switch(powerupType) {
            case PowerupType.Life:
                currentLife = Math.Min(currentLife + 50f, 100f);
                healthObj.SetLife(currentLife);
                break;
            case PowerupType.Damage:
                TakeDamage(20f);
                break;
            case PowerupType.Bullet:
                bulletPowerup = 5f;
                break;
        }
    }

    private void TakeDamage(float damageValue)
    {
        currentLife -= damageValue;
        healthObj.SetLife(currentLife);
        if (currentLife <= 0)
        {
            // Handle player death
            OnDeath?.Invoke();
            currentLife = 100f;
            healthObj.SetLife(currentLife);
        }
    }

    public void StopInput(bool win)
    {
        inputActions.Disable();
        gameRunning = false;
        if (!win)
        {
            playerRigidbody.simulated = false;
            playerCollider.enabled = false;
            playerAnimator.SetBool("Dead", true);
            healthBar.SetActive(false);
        }
    }
}
