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

    private float timeRemaining = 0f;

    private bool gameRunning = true;
    public bool GameRunning => gameRunning;
    public GameVariablesSO GameVariables => gameVariables;
    public PlayerController Player => playerController;

    private void Awake()
    {
        timeRemaining = gameVariables.timeToWin;
        enemySpawnerManager.Initialize(gameObjectsPoolManager, this);
    }
    public void SetPlayer(PlayerController playerObj)
    {
        playerController = playerObj;
        playerController.OnAttack += HandlePlayerAttack;
        playerController.OnHit += HandlePlayerHit;
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
    private void HandlePlayerHit()
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
        if(gameVariables.winCondition == WinCondition.Time)
        {
            timeRemaining -= Time.deltaTime;
            if (timeRemaining <= 0) timeRemaining = 0f;
            gameUIManager.UpdateTimer(timeRemaining);
            if (timeRemaining <= 0)
            {
                Debug.Log("Player win, time ran out");
                GameFinished(true);
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
        playerPoints++;
        gameUIManager.UpdatePoints(playerPoints);
        if (gameVariables.winCondition == WinCondition.Points && playerPoints >= gameVariables.pointsToWin)
        {
            Debug.Log("Player win, scored enough points");
            GameFinished(true);
        }
    }
    public void SetAudioManager(AudioManager mainMenuAudio)
    {
        audioManager = mainMenuAudio;
        audioManager.SetVolume(ServiceLocator.GetService<ISaveService>().GetVolume());
    }

}
