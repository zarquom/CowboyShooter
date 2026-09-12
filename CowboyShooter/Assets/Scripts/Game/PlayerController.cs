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
    public event Action OnAttack;
    public event Action OnDeath;
    private InputSystem_Actions inputActions;

    private float currentLife = 100f;
    private bool gameRunning = true;
    void Start()
    {
        inputActions = new InputSystem_Actions();
        inputActions.Enable();
        inputActions.Player.Attack.performed += OnAttackPerformed;
    }

    void OnDestroy()
    {
        inputActions.Player.Attack.performed -= OnAttackPerformed;
        inputActions.Disable();
    }

    private void OnAttackPerformed(InputAction.CallbackContext context)
    {
        OnAttack?.Invoke();
    }

    void FixedUpdate()
    {
        HandleMovement();
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
            collision.gameObject.GetComponent<BulletController>().DeactivateBullet();
            Instantiate(ServiceLocator.GetService<IAssetLoader>().GetAsset<GameObject>("BulletSparkles"), transform.position, transform.rotation);
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
            healthObj.gameObject.SetActive(false);
        }
    }
}
