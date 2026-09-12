using UnityEngine;
using UnityEngine.Pool;

public class GameObjectsPoolManager : MonoBehaviour
{
    private ObjectPool<EnemyController> enemyObjectPool;
    private ObjectPool<BulletController> bulletObjectPool;
    private ObjectPool<PowerupController> powerupObjectPool;
    private ObjectPool<BossController> bossObjectPool;

    void Start()
    {
        enemyObjectPool = new ObjectPool<EnemyController>(
            createFunc: () => Instantiate(ServiceLocator.GetService<IAssetLoader>().GetAsset<GameObject>("Enemy")).GetComponent<EnemyController>(),
            actionOnGet: (enemy) => enemy.gameObject.SetActive(true),
            actionOnRelease: (enemy) => enemy.gameObject.SetActive(false),
            actionOnDestroy: (enemy) => Destroy(enemy.gameObject),
            collectionCheck: false,
            defaultCapacity: 20,
            maxSize: 40
        );
        bulletObjectPool = new ObjectPool<BulletController>(
            createFunc: () => Instantiate(ServiceLocator.GetService<IAssetLoader>().GetAsset<GameObject>("Bullet")).GetComponent<BulletController>(),
            actionOnGet: (bullet) => bullet.gameObject.SetActive(true),
            actionOnRelease: (bullet) => bullet.gameObject.SetActive(false),
            actionOnDestroy: (bullet) => Destroy(bullet.gameObject),
            collectionCheck: false,
            defaultCapacity: 40,
            maxSize: 100
        );
        powerupObjectPool = new ObjectPool<PowerupController>(
            createFunc: () => Instantiate(ServiceLocator.GetService<IAssetLoader>().GetAsset<GameObject>("Powerup")).GetComponent<PowerupController>(),
            actionOnGet: (powerup) => powerup.gameObject.SetActive(true),
            actionOnRelease: (powerup) => powerup.gameObject.SetActive(false),
            actionOnDestroy: (powerup) => Destroy(powerup.gameObject),
            collectionCheck: false,
            defaultCapacity: 10,
            maxSize: 20
        );
        bossObjectPool = new ObjectPool<BossController>(
            createFunc: () => Instantiate(ServiceLocator.GetService<IAssetLoader>().GetAsset<GameObject>("Boss")).GetComponent<BossController>(),
            actionOnGet: (boss) => boss.gameObject.SetActive(true),
            actionOnRelease: (boss) => boss.gameObject.SetActive(false),
            actionOnDestroy: (boss) => Destroy(boss.gameObject),
            collectionCheck: false,
            defaultCapacity: 10,
            maxSize: 20
        );
    }

    public EnemyController GetEnemyFromPool()
    {
        return enemyObjectPool.Get();
    }

    public BulletController GetBulletFromPool()
    {
        return bulletObjectPool.Get();
    }
    public PowerupController GetPowerupFromPool()
    {
        return powerupObjectPool.Get();
    }
    public BossController GetBossFromPool()
    {
        return bossObjectPool.Get();
    }
    public void ReleaseEnemyToPool(EnemyController enemy)
    {
        enemyObjectPool.Release(enemy);
    }

    public void ReleaseBulletToPool(BulletController bullet)
    {
        bulletObjectPool.Release(bullet);
    }
    public void ReleasePowerupToPool(PowerupController powerup)
    {
        powerupObjectPool.Release(powerup);
    }
    public void ReleaseBossToPool(BossController boss)
    {
        bossObjectPool.Release(boss);
    }
}
