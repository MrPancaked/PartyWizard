using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private GameObject settingsUI;
    [SerializeField] private GameObject cheatUI;
    [SerializeField] private GameObject victoryUI;
    [SerializeField] private GameObject pauseGameUI;
    [SerializeField] private GameObject playerDeathUI;
    [SerializeField] private GameObject enemyParent;
    [SerializeField] private TextMeshProUGUI enemyCounter;
    [SerializeField] private TextMeshProUGUI roundCounter;
    [SerializeField] private List<EnemyWaveData> enemyWaves;

    [Header("GameState Variables")] 
    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] private int waitTime;
    
    public int enemyCount {get; private set;}
    private int round;

    public Action RoomClearedEvent;
    public Action RoomStartEvent;
    public Action PauseEvent;
    public Action UnPauseEvent;
    public Action<EnemyWaveData> SpawnEnemiesEvent;

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
        
        if (InputManager.Instance != null)
        {
            InputManager.Instance.PauseGameAction.performed += PauseGame;
            InputManager.Instance.CheatAction.performed += ToggleCheatMenu;
        }
    }

    private void OnDisable()
    {
        EventBus<EnemySpawnEventData>.OnEventPublished -= OnEnemySpawned;
        EventBus<EnemyDieEventData>.OnEventPublished -= OnEnemyDied;
        EventBus<PlayerDieEventData>.OnEventPublished -= OnPlayerDied;

        if (InputManager.Instance != null)
        {
            InputManager.Instance.PauseGameAction.performed -= PauseGame; // somettimes this gets called after 
            InputManager.Instance.CheatAction.performed -= ToggleCheatMenu;
        }
    }

    private void Start()// ONLY TO COUNT ENEMIES FOR TESTING RN, CHANGE LATER WHEN SPAWNERS ARE ADDED
    {
        Time.timeScale = 1f;
        
        countdownText.gameObject.SetActive(false);
        pauseGameUI.SetActive(false);
        playerDeathUI.SetActive(false);
        victoryUI.SetActive(false);
        settingsUI.SetActive(false);
        
        CountEnemies();
        
        EventInstance musicInstace = AudioManager.Instance.CreateInstance(FMODEvents.Instance.music);
        musicInstace.start();
        
        StartCoroutine(NewRound());
    }

    public IEnumerator NewRound()
    {
        ClearItems();
        RoomStartEvent?.Invoke();
        
        round++;
        roundCounter.text = $"Round: {round}";
        
        yield return StartCoroutine(NewRoundAnimation());
        Debug.Log("New round");
        
        SpawnEnemiesEvent?.Invoke(enemyWaves[round - 1]); // I'm not sure I can guarantee if all subscribers excute before the next line, so far so good though.
        CountEnemies();
    }
    public void OnEnemyDied(EnemyDieEventData enemyDieEventData)
    {
        enemyCount--;
        enemyCounter.text = $"{enemyCount}";
        
        if (enemyCount == 0)
        {
            Debug.Log("All enemies have been Killed");
            if (round < enemyWaves.Count)
            {
                RoomClearedEvent?.Invoke();
                AudioManager.Instance.PlayOneShot(FMODEvents.Instance.doorOpenSounds, Vector2.zero);
            }
            else
            {
                PauseEvent?.Invoke();
                AudioManager.Instance.PlayOneShot(FMODEvents.Instance.winSound, Vector2.zero);
                victoryUI.SetActive(true);
                Time.timeScale = 0f;
            }
        }
    }

    public void OnEnemySpawned(EnemySpawnEventData enemySpawnEventData)
    {
        enemyCount++;
        enemyCounter.text = $"{enemyCount}";
    }

    public void OnPlayerDied(PlayerDieEventData playerDieEventData)
    {
        playerDeathUI.SetActive(true);
        PauseEvent?.Invoke();
        Time.timeScale = 0f;
    }
    
    private void CountEnemies()
    {
        enemyCount = enemyParent.GetComponentsInChildren<HpController>().Length;
        enemyCounter.text = $"{enemyCount}";
    }

    private void ClearItems()
    {
        LayerMask layerMask = LayerMask.GetMask("Xp");
        layerMask |= LayerMask.GetMask("Item");
        Collider2D[] colliders = Physics2D.OverlapBoxAll(new Vector2(0,0), new Vector2(14f, 14f), 0, layerMask);
        foreach (Collider2D hit in colliders)
        {
            Destroy(hit.gameObject);
        }
    }
    public void ToggleCheatMenu(InputAction.CallbackContext context)
    {
        if (!cheatUI.activeInHierarchy)
        {
            cheatUI.SetActive(true);
        }
        else 
        {
            cheatUI.SetActive(false);
        }
    }

    public void OpenSettings()
    {
        settingsUI.SetActive(true);
    }
        
    #region PauseGame
    public void TogglePause()
    {
        if (!pauseGameUI.activeInHierarchy)
        {
            pauseGameUI.SetActive(true);
            Time.timeScale = 0f;
            PauseEvent?.Invoke();
        }
        else if (!settingsUI.activeInHierarchy)
        {
            pauseGameUI.SetActive(false);
            Time.timeScale = 1f;
            UnPauseEvent?.Invoke();
        }
        else settingsUI.SetActive(false);
    }
    public void PauseGame(InputAction.CallbackContext context)
    {
        if (context.performed && !playerDeathUI.activeInHierarchy)
            TogglePause();
    }
    public void PauseFromButton()
    {
        TogglePause();
    }
    #endregion
    
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
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
    #endregion
    
    #region GameState Animations

    private IEnumerator NewRoundAnimation()
    {
        countdownText.gameObject.SetActive(true);
        int i = waitTime;
        while (i > 0)
        {
            countdownText.text = $"{i}";
            yield return new WaitForSeconds(1f);
            i--;
        }
        yield return new WaitForEndOfFrame();
        countdownText.text = $"GO!";
        yield return new WaitForSeconds(1f);
        countdownText.gameObject.SetActive(false);
    }
    
    #endregion
}
