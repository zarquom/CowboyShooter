using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Rigidbody2D playerRigidbody;
    public event Action OnAttack;
    public event Action OnDeath;
    private InputSystem_Actions inputActions;

    private float currentLife = 100f;
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
        if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(1f);
        }

    }

    private void TakeDamage(float damageValue)
    {
        currentLife -= damageValue;
        if (currentLife <= 0)
        {
            // Handle player death
            OnDeath?.Invoke();
            currentLife = 100f;
        }
    }

    public void StopInput()
    {
        inputActions.Disable();
    }
}
