using System;
using System.Collections.Generic;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private GameObject playerDeathUI;
    [SerializeField] private GameObject enemyParent;
    [SerializeField] private TextMeshProUGUI enemyCountText;
    [SerializeField] private List<GameObject> enemies;
    
    private int enemyCount;

    public Action RoomClearedEvent;
    public Action<List<GameObject>> RoomStartEvent;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; } // Singleton
        Instance = this;
    }
    private void OnEnable()
    {
        EventBus<EnemySpawnEventData>.OnEventPublished += OnEnemySpawned;
        EventBus<EnemyDieEventData>.OnEventPublished += OnEnemyDied;
        EventBus<PlayerDieEventData>.OnEventPublished += OnPlayerDied;
    }

    private void OnDisable()
    {
        EventBus<EnemySpawnEventData>.OnEventPublished -= OnEnemySpawned;
        EventBus<EnemyDieEventData>.OnEventPublished -= OnEnemyDied;
        EventBus<PlayerDieEventData>.OnEventPublished -= OnPlayerDied;
    }

    private void Start()// ONLY TO COUNT ENEMIES FOR TESTING RN, CHANGE LATER WHEN SPAWNERS ARE ADDED
    {
        playerDeathUI.SetActive(false);
        NewRound();
        CountEnemies();
    }

    private void NewRound()
    {
        RoomStartEvent?.Invoke(enemies);
    }
    public void OnEnemyDied(EnemyDieEventData enemyDieEventData)
    {
        enemyCount--;
        enemyCountText.text = $"{enemyCount}";
        
        if (enemyCount == 0)
        {
            Debug.Log("All enemies have been Killed");
            RoomClearedEvent?.Invoke();
        }
    }

    public void OnEnemySpawned(EnemySpawnEventData enemySpawnEventData)
    {
        enemyCount++;
        enemyCountText.text = $"{enemyCount}";
    }

    public void OnPlayerDied(PlayerDieEventData playerDieEventData)
    {
        playerDeathUI.SetActive(true);
    }
    
    private void CountEnemies()
    {
        enemyCount = enemyParent.GetComponentsInChildren<HpController>().Length;
        enemyCountText.text = $"{enemyCount}";
    }

    #region Scene Management
    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
    #endregion
}
