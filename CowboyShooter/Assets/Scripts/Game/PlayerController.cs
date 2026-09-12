using System;
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

    private GameVariablesSO gameVariables;
    private float currentLife;
    private float bulletPowerup = 0f;
    private bool gameRunning = true;
    private GameObject bulletSparklesPrefab;
    private GameObject powerupSparklesPrefab;

    public void Initialize(GameVariablesSO variables)
    {
        gameVariables = variables;
        currentLife = gameVariables.playerMaxLife;
    }

    void Start()
    {
        inputActions = new InputSystem_Actions();
        inputActions.Enable();
        inputActions.Player.Attack.performed += OnAttackPerformed;
        bulletPowerup = 0f;
        bulletSparklesPrefab = ServiceLocator.GetService<IAssetLoader>().GetAsset<GameObject>("BulletSparkles");
        powerupSparklesPrefab = ServiceLocator.GetService<IAssetLoader>().GetAsset<GameObject>("PowerupSparkles");
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
            TakeDamage(gameVariables.playerEnemyContactDamage);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            TakeDamage(gameVariables.playerBulletDamage);
            OnHit?.Invoke();
            collision.gameObject.GetComponent<BulletController>().DeactivateBullet();
            Instantiate(bulletSparklesPrefab, transform.position, transform.rotation);
        }
        if (collision.gameObject.CompareTag("Powerup"))
        {
            PowerupController powerupController = collision.gameObject.GetComponent<PowerupController>();
            PowerupEffect(powerupController.PowerupType);
            powerupController.DeactivatePowerup(true);
            Instantiate(powerupSparklesPrefab, transform.position, transform.rotation);
        }
    }

    private void PowerupEffect(PowerupType powerupType)
    {
        switch(powerupType) {
            case PowerupType.Life:
                currentLife = Math.Min(currentLife + gameVariables.playerLifePowerupHealAmount, gameVariables.playerMaxLife);
                healthObj.SetLife(currentLife, gameVariables.playerMaxLife);
                break;
            case PowerupType.Damage:
                TakeDamage(gameVariables.playerDamagePowerupAmount);
                break;
            case PowerupType.Bullet:
                bulletPowerup = gameVariables.bulletPowerupDuration;
                break;
        }
    }

    private void TakeDamage(float damageValue)
    {
        currentLife -= damageValue;
        healthObj.SetLife(currentLife, gameVariables.playerMaxLife);
        if (currentLife <= 0)
        {
            // Handle player death
            OnDeath?.Invoke();
            currentLife = gameVariables.playerMaxLife;
            healthObj.SetLife(currentLife, gameVariables.playerMaxLife);
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
