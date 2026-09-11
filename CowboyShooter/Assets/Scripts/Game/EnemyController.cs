using System;
using UnityEngine;
public class EnemyController : MonoBehaviour
{
    [SerializeField] private EnemyMovementController enemyMovementController;
    [SerializeField] private Animator horseAnimator;
    [SerializeField] private AnimatorOverrideController[] horseAnimatorOverrides;
    public event Action<EnemyController> OnEnemyDeactivated;
    public event Action<EnemyController> OnEnemyDestroyed;
    public event Action<Transform> OnAttack;

    private float timeToAttack = 2f;
    private float timerAttack = 0f;
    private bool canBeDestroyed = false;
    private EnemyType enemyType;
    public EnemyType EnemyType => enemyType;

    public void Initialize(EnemyType type, GameVariablesSO gameVariables, PlayerController player)
    {
        enemyType = type;
        transform.localScale = new Vector3(2f, 2f, 1f);
        horseAnimator.runtimeAnimatorController = horseAnimatorOverrides[(int)type];
        if(enemyType == EnemyType.Strong)
        {
            transform.localScale = new Vector3(3.5f, 3.5f, 1f);
        }
        enemyMovementController.Initialize(enemyType, gameVariables, player);
    }

    private void Update()
    {
        CheckBounds();
        CheckAttack();
    }

    private void CheckAttack()
    {
        timerAttack += Time.deltaTime;
        if(timerAttack >= timeToAttack)
        {
            OnAttack?.Invoke(transform);
            timerAttack = 0f;
            timeToAttack = UnityEngine.Random.Range(3f, 6f); // Randomize the next attack time
        }
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
        if (collision.gameObject.CompareTag("Bullet") && canBeDestroyed)
        {
            OnEnemyDestroyed?.Invoke(this);
            //Add animation or effects here
            Deactivate();
            collision.gameObject.GetComponent<BulletController>().DeactivateBullet();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Destroyable"))
        {
            canBeDestroyed = true;
        }
    }

    private void Deactivate()
    {
        OnEnemyDeactivated?.Invoke(this);
        canBeDestroyed = false;
        enemyMovementController.RemoveMovement();
    }
}

public enum EnemyType
{
    Basic,
    Fast,
    Strong
}