using System;
using System.Collections;
using UnityEngine;
public class BossController : MonoBehaviour
{
    [SerializeField] private Animator bossAnimator;
    [SerializeField] private Rigidbody2D bossRigidbody;
    public event Action<BossController> OnEnemyDestroyed;
    public event Action<BossController> OnEnemyDeactivated;
    public event Action<Transform> OnAttack;
    public event Action OnHit;

    private MovementStrategy movement;
    private MovementState state = new MovementState();
    private float timeToAttack = 2f;
    private float timerAttack = 0f;
    private bool canBeDestroyed = false;
    private int lifeHits;
    private GameObject bulletSparklesPrefab;

    public void Initialize(bool bigBoss, GameVariablesSO gameVariables, PlayerController player)
    {
        lifeHits = bigBoss ? gameVariables.bossMaxLife : gameVariables.bossNormalLife;
        movement = gameVariables.bossMovement;
        state.startPosition = new Vector3(UnityEngine.Random.Range(-1f, 1f), 3f, 0f);
        if (bulletSparklesPrefab == null)
        {
            bulletSparklesPrefab = ServiceLocator.GetService<IAssetLoader>().GetAsset<GameObject>("BulletSparkles");
        }
    }

    private void Update()
    {
        CheckAttack();
    }
    void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        if (movement == null) return;
        Vector2 velocity = movement.GetVelocity(bossRigidbody, ref state, Time.fixedDeltaTime);
        bossRigidbody.linearVelocity = velocity;
    }
    private void CheckAttack()
    {
        timerAttack += Time.deltaTime;
        if(timerAttack >= timeToAttack)
        {
            bossAnimator.SetTrigger("Attack");
            OnAttack?.Invoke(transform);
            timerAttack = 0f;
            timeToAttack = UnityEngine.Random.Range(2f, 4f); // Randomize the next attack time
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
                bossAnimator.SetBool("Dead", true);
                Deactivate();
            } else
            {
                bossAnimator.SetTrigger("Hit");
                OnHit?.Invoke();
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
        OnEnemyDestroyed?.Invoke(this);
        StartCoroutine(DeactivateAfterAnimation());
        canBeDestroyed = false;
    }

    IEnumerator DeactivateAfterAnimation()
    {
        yield return new WaitForSeconds(0.8f); // Wait for the death animation to finish
        if (this == null) yield break;
        OnHit = null;
        OnEnemyDeactivated?.Invoke(this);
    }
}