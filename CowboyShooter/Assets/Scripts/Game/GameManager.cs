using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameVariablesSO gameVariables;
    [SerializeField] private GameObjectsPoolManager gameObjectsPoolManager;
    [SerializeField] private EnemySpawnerManager enemySpawnerManager;
    private PlayerController playerController;
    private GameUIManager gameUIManager;

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
        playerController.OnDeath += HandlePlayerDeath;
    }
    public void SetGameUI(GameUIManager uiManager)
    {
        gameUIManager = uiManager;
        gameUIManager.Initialize(playerPoints, playerLives);
        gameUIManager.OnPlayAgainClicked += HandlePlayAgainClicked;
        gameUIManager.OnMenuClicked += HandleMenuClicked;
    }

    private void HandleMenuClicked()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void HandlePlayAgainClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void HandlePlayerAttack()
    {
        BulletController bullet = gameObjectsPoolManager.GetBulletFromPool();
        bullet.transform.position = playerController.transform.position;
        bullet.OnBulletDeactivated += HandleBulletDeactivated;
    }
    private void HandlePlayerDeath()
    {
        playerLives--;
        gameUIManager.UpdateLives(playerLives);
        if (playerLives <= 0)
        {
            Debug.Log("Game Over, player lost all lives");
            GameFinished(false);
        }
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
        gameRunning = false;
        playerController.StopInput();
        gameUIManager.ShowGameOverScreen(win, playerPoints, gameVariables.winCondition == WinCondition.Time);
    }

    public void OnEnemyDestroyed(EnemyController enemy)
    {
        Debug.Log($"Enemy destroyed: {enemy.EnemyType}");
        playerPoints++;
        gameUIManager.UpdatePoints(playerPoints);
        if (gameVariables.winCondition == WinCondition.Points && playerPoints >= gameVariables.pointsToWin)
        {
            Debug.Log("Player win, scored enough points");
            GameFinished(true);
        }
    }
}
