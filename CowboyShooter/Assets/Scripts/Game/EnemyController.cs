using System;
using System.Collections;
using UnityEngine;
public class EnemyController : MonoBehaviour
{
    [SerializeField] private EnemyMovementController enemyMovementController;
    [SerializeField] private Animator horseAnimator;
    [SerializeField] private AnimatorOverrideController[] horseAnimatorOverrides;
    public event Action<EnemyController> OnEnemyDeactivated;
    public event Action<EnemyController> OnEnemyDestroyed;
    public event Action<Transform> OnAttack;

    private float timeToAttack;
    private float timerAttack = 0f;
    private bool canBeDestroyed = false;
    private int lifeHits;
    private EnemyType enemyType;
    private GameVariablesSO gameVariables;
    private GameObject bulletSparklesPrefab;
    public EnemyType EnemyType => enemyType;

    public void Initialize(EnemyType type, GameVariablesSO gameVariables, PlayerController player)
    {
        this.gameVariables = gameVariables;
        enemyType = type;
        timeToAttack = gameVariables.enemyAttackInitialDelay;
        lifeHits = gameVariables.enemyBasicLifeHits;
        transform.localScale = new Vector3(gameVariables.enemyBasicScale, gameVariables.enemyBasicScale, 1f);
        horseAnimator.runtimeAnimatorController = horseAnimatorOverrides[(int)type];
        horseAnimator.SetBool("Dead", false);
        if (enemyType == EnemyType.Strong)
        {
            transform.localScale = new Vector3(gameVariables.enemyStrongScale, gameVariables.enemyStrongScale, 1f);
            lifeHits = gameVariables.enemyStrongLifeHits;
        }
        enemyMovementController.Initialize(enemyType, gameVariables, player);
        enemyMovementController.ActivateRigidbody(true);
        if(bulletSparklesPrefab == null)
        {
            bulletSparklesPrefab = ServiceLocator.GetService<IAssetLoader>().GetAsset<GameObject>("BulletSparkles");
        }
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
            timeToAttack = UnityEngine.Random.Range(gameVariables.enemyAttackIntervalMin, gameVariables.enemyAttackIntervalMax); // Randomize the next attack time
        }
    }

    private void CheckBounds()
    {
        if (transform.position.y < gameVariables.enemyDespawnMinY || transform.position.x > gameVariables.enemyDespawnMaxX || transform.position.x < -gameVariables.enemyDespawnMaxX)
        {
            Deactivate();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet") && canBeDestroyed)
        {
            lifeHits--;
            Instantiate(bulletSparklesPrefab, transform.position, transform.rotation);
            if (lifeHits <= 0)
            {
                OnEnemyDestroyed?.Invoke(this);
                Deactivate();
            }
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
        if(!canBeDestroyed) return;
        StartCoroutine(DeactivateAfterAnimation());
        canBeDestroyed = false;
        enemyMovementController.ActivateRigidbody(false);
        enemyMovementController.RemoveMovement();
        horseAnimator.SetBool("Dead", true);
    }

    IEnumerator DeactivateAfterAnimation()
    {
        yield return new WaitForSeconds(gameVariables.enemyDeathAnimationDuration); // Wait for the death animation to finish
        if (this == null) yield break;
        OnEnemyDeactivated?.Invoke(this);
        horseAnimator.gameObject.transform.localRotation = Quaternion.identity;
        horseAnimator.gameObject.transform.localScale = Vector2.one;
    }
}

public enum EnemyType
{
    Basic,
    Fast,
    Strong
}