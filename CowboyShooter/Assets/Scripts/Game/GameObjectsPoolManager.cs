using UnityEngine;
using UnityEngine.Pool;

public class GameObjectsPoolManager : MonoBehaviour
{
    private ObjectPool<EnemyController> enemyObjectPool;
    private ObjectPool<BulletController> bulletObjectPool;

    void Start()
    {
        enemyObjectPool = new ObjectPool<EnemyController>(
            createFunc: () => Instantiate(ServiceLocator.GetService<AssetLoaderManager>().GetAsset<GameObject>("Enemy")).GetComponent<EnemyController>(),
            actionOnGet: (enemy) => enemy.gameObject.SetActive(true),
            actionOnRelease: (enemy) => enemy.gameObject.SetActive(false),
            actionOnDestroy: (enemy) => Destroy(enemy.gameObject),
            collectionCheck: false,
            defaultCapacity: 10,
            maxSize: 20
        );
        bulletObjectPool = new ObjectPool<BulletController>(
            createFunc: () => Instantiate(ServiceLocator.GetService<AssetLoaderManager>().GetAsset<GameObject>("Bullet")).GetComponent<BulletController>(),
            actionOnGet: (bullet) => bullet.gameObject.SetActive(true),
            actionOnRelease: (bullet) => bullet.gameObject.SetActive(false),
            actionOnDestroy: (bullet) => Destroy(bullet.gameObject),
            collectionCheck: false,
            defaultCapacity: 20,
            maxSize: 40
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
}
