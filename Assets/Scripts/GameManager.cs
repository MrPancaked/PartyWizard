using System;
using Player;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private GameObject playerDeathUI;
    [SerializeField] private GameObject enemyParent;
    [SerializeField] private TextMeshProUGUI enemyCountText;
    private int enemyCount;

    private void Start()// ONLY TO COUNT ENEMIES FOR TESTING RN, CHANGE LATER WHEN SPAWNERS ARE ADDED
    {
        playerDeathUI.SetActive(false);
        CountEnemies();
    }

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; } // Singleton
        Instance = this;
    }
    public void OnEnemyDied()
    {
        enemyCount--;
        enemyCountText.text = $"{enemyCount}";
        
        if (enemyCount == 0)
        {
            Debug.Log("All enemies have been Killed");
            //open doors to next chambers
        }
    }

    public void OnPlayerDied()
    {
        playerDeathUI.SetActive(true);
    }

    private void CountEnemies()
    {
        enemyCount = enemyParent.GetComponentsInChildren<HpController>().Length;
        enemyCountText.text = $"{enemyCount}";
    }
    
}
