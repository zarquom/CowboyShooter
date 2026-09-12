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

    private float timeToAttack = 2f;
    private float timerAttack = 0f;
    private bool canBeDestroyed = false;
    private int lifeHits;
    private EnemyType enemyType;
    private GameObject bulletSparklesPrefab;
    public EnemyType EnemyType => enemyType;

    public void Initialize(EnemyType type, GameVariablesSO gameVariables, PlayerController player)
    {
        enemyType = type;
        lifeHits = 1;
        transform.localScale = new Vector3(2f, 2f, 1f);
        horseAnimator.runtimeAnimatorController = horseAnimatorOverrides[(int)type];
        horseAnimator.SetBool("Dead", false);
        if (enemyType == EnemyType.Strong)
        {
            transform.localScale = new Vector3(2.5f, 2.5f, 1f);
            lifeHits = 3;
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
            timeToAttack = UnityEngine.Random.Range(3f, 6f); // Randomize the next attack time
        }
    }

    private void CheckBounds()
    {
        if (transform.position.y < -10f || transform.position.x > 15f || transform.position.x < -15f)
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
        yield return new WaitForSeconds(0.5f); // Wait for the death animation to finish
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