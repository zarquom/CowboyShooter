using UnityEngine;
public class EnemyMovementController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D enemyRigidbody;
    private EnemyType enemyType;
    private PlayerController playerController;
    private MovementStrategy movement;
    private MovementState state = new MovementState();
    public void Initialize(EnemyType type, GameVariablesSO gameVariables, PlayerController player)
    {
        enemyType = type;
        playerController = player;
        state.startPosition = transform.position;
        AssignMovementStrategy(gameVariables);
        if(movement is HomingMovement)
        {
            (movement as HomingMovement).target = playerController.transform;
        }
        if (movement is OrbitMovement)
        {
            state.startPosition = new Vector3(UnityEngine.Random.Range(-1f, 1f), 3f, 0f);
        }
    }

    private void AssignMovementStrategy(GameVariablesSO gameVariables)
    {
        switch (enemyType)
        {
            case EnemyType.Basic:
                movement = gameVariables.basicEnemyMovement;
                break;
             case EnemyType.Fast:
                movement = gameVariables.fastEnemyMovement;
                break;
             case EnemyType.Strong:
                movement = gameVariables.strongEnemyMovement;
                break;
        }
    }

    void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        if(movement == null) return;
        Vector2 velocity = movement.GetVelocity(enemyRigidbody, ref state, Time.fixedDeltaTime);
        enemyRigidbody.linearVelocity = velocity;
    }

    public void RemoveMovement()
    {
        movement = null;
        state = new MovementState();
    }

    public void ActivateRigidbody(bool activate)
    {
        enemyRigidbody.simulated = activate;
    }
}