using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameVariablesSO gameVariables;
    [SerializeField] private GameObjectsPoolManager gameObjectsPoolManager;
    [SerializeField] private EnemySpawnerManager enemySpawnerManager;
    private PlayerController playerController;
    private GameUIManager gameUIManager;
    private AudioManager audioManager;

    private int playerPoints = 0;
    private int playerLives = 3;

    private float timePassed = 0f;
    private float bossTimer = 0f;
    private bool bigBossSpawned = false;

    private bool gameRunning = true;
    public bool GameRunning => gameRunning;
    public GameVariablesSO GameVariables => gameVariables;
    public PlayerController Player => playerController;

    private void Awake()
    {
        timePassed = 0f;
        enemySpawnerManager.Initialize(gameObjectsPoolManager, this);
    }
    public void SetPlayer(PlayerController playerObj)
    {
        playerController = playerObj;
        playerController.OnAttack += HandlePlayerAttack;
        playerController.OnHit += HandleBulletHit;
        playerController.OnDeath += HandlePlayerDeath;
    }
    public void SetGameUI(GameUIManager uiManager)
    {
        gameUIManager = uiManager;
        gameUIManager.Initialize(playerPoints, playerLives, audioManager);
        gameUIManager.OnPlayAgainClicked += HandlePlayAgainClicked;
        gameUIManager.OnMenuClicked += HandleMenuClicked;
    }

    private void HandleMenuClicked()
    {
        audioManager.PlaySound("Button1");
        StartCoroutine(LoadSceneWithDelay("MainMenu"));
    }

    private void HandlePlayAgainClicked()
    {
        audioManager.PlaySound("Button2");
        StartCoroutine(LoadSceneWithDelay(SceneManager.GetActiveScene().name));
    }

    IEnumerator LoadSceneWithDelay(string sceneName)
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(sceneName);
    }

    private void HandlePlayerAttack()
    {
        if (!gameRunning) return;
        BulletController bullet = gameObjectsPoolManager.GetBulletFromPool();
        bullet.transform.position = playerController.transform.position;
        bullet.OnBulletDeactivated += HandleBulletDeactivated;
        bullet.Initialize(BulletType.Player, gameVariables, Vector2.up);
    }
    public void HandleEnemyAttack(Transform enemyTransform)
    {
        if (!gameRunning) return;
        BulletController bullet = gameObjectsPoolManager.GetBulletFromPool();
        bullet.transform.position = enemyTransform.position;
        bullet.OnBulletDeactivated += HandleBulletDeactivated;
        bullet.Initialize(BulletType.Enemy, gameVariables, (playerController.transform.position - enemyTransform.position).normalized);
    }
    private void HandlePlayerDeath()
    {
        if(!gameRunning) return;
        playerLives--;
        audioManager.PlaySound("Hit");
        gameUIManager.UpdateLives(playerLives);
        if (playerLives <= 0)
        {
            Debug.Log("Game Over, player lost all lives");
            GameFinished(false);
        }
    }
    public void HandleBulletHit()
    {
        if(!gameRunning) return;
        audioManager.PlaySound("Blop");
    }

    private void HandleBulletDeactivated(BulletController bullet)
    {
        bullet.OnBulletDeactivated -= HandleBulletDeactivated;
        gameObjectsPoolManager.ReleaseBulletToPool(bullet);
    }

    private void Update()
    {
        if(!gameRunning) return;
        TimerCheck();
    }

    private void TimerCheck()
    {
        timePassed += Time.deltaTime;
        if (gameVariables.winCondition == WinCondition.Time)
        {
            float timeRemaining = gameVariables.timeToWin - timePassed;
            if (timeRemaining <= 0) timeRemaining = 0f;
            gameUIManager.UpdateTimer(timeRemaining);
            if (timeRemaining <= 0)
            {
                Debug.Log("Player win, time ran out");
                GameFinished(true);
            }
        }
        if(gameVariables.winCondition == WinCondition.BossDefeat)
        {
            if (!bigBossSpawned && timePassed >= gameVariables.timeForBossAppearanceBossMode)
            {
                Debug.Log("Boss time reached, spawning boss");
                enemySpawnerManager.SpawnBoss(true);
                bigBossSpawned = true;
                enemySpawnerManager.SetSpawnInterval(6f, 0f);
            }
        } else
        {
            bossTimer += Time.deltaTime;
            if (bossTimer >= gameVariables.timeForBossAppearanceOtherModes)
            {
                Debug.Log("Boss time reached, spawning boss");
                enemySpawnerManager.SpawnBoss();
                enemySpawnerManager.SetSpawnInterval(6f, 0f);
                bossTimer = 0f;
            }
        }
    }

    private void GameFinished(bool win)
    {
        audioManager.PlaySound(win ? "Win" : "Gameover");
        gameRunning = false;
        playerController.StopInput(win);
        gameUIManager.ShowGameOverScreen(win, playerPoints, gameVariables.winCondition == WinCondition.Time);
    }

    public void OnEnemyDestroyed(EnemyController enemy)
    {
        Debug.Log($"Enemy destroyed: {enemy.EnemyType}");
        audioManager.PlaySound("Explosion");
        int pointsEarned = 1;
        if(enemy.EnemyType == EnemyType.Strong)
        {
            pointsEarned = 5;
        }
        playerPoints += pointsEarned;
        gameUIManager.UpdatePoints(playerPoints);
        if (gameVariables.winCondition == WinCondition.Points && playerPoints >= gameVariables.pointsToWin)
        {
            Debug.Log("Player win, scored enough points");
            GameFinished(true);
        }
    }
    public void OnBossDestroyed(BossController boss)
    {
        Debug.Log($"Boss destroyed: {gameVariables.winCondition}");
        audioManager.PlaySound("Explosion");
        int pointsEarned = 50;
        playerPoints += pointsEarned;
        gameUIManager.UpdatePoints(playerPoints);
        if (gameVariables.winCondition == WinCondition.Points && playerPoints >= gameVariables.pointsToWin)
        {
            Debug.Log("Player win, scored enough points");
            GameFinished(true);
        } else if(gameVariables.winCondition == WinCondition.BossDefeat)
        {
            Debug.Log("Player win, defeated the boss");
            GameFinished(true);
        }
        else // Other win conditions, will continue the game after boss defeat
        {
            enemySpawnerManager.SetSpawnInterval(2f, gameVariables.enemySpawnIncreaseRate);
        }
    }
    public void SetAudioManager(AudioManager mainMenuAudio)
    {
        audioManager = mainMenuAudio;
        audioManager.SetVolume(ServiceLocator.GetService<ISaveService>().GetVolume());
    }

}
