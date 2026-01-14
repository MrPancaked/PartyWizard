using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using Player;
using Random = UnityEngine.Random;
/// <summary>
/// Class that spawns enemy waves (and individual enemies)
/// Takes waves as arrays of GameObjects.
/// Can be used to spawn enemies at the start of the round, make other enemies spawn more enemies
/// </summary>
public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private float playerRange;
    [SerializeField] private GameObject skullEnemy;
    [SerializeField] private GameObject knightEnemy;
    [SerializeField] private GameObject bossEnemy;
    [SerializeField] private Transform enemyParent;
    [SerializeField] private Rect spawnArea;

    private int spawnAmount = 1;
    private GameManager gameManager;
    
    private void Start()
    {
        gameManager = GameManager.Instance;
        if (gameManager != null)
            gameManager.SpawnEnemiesEvent += SpawnEnemies;
    }

    private void OnDisable()
    {
        if (gameManager != null)
            gameManager.SpawnEnemiesEvent -= SpawnEnemies;
    }

    //method to find the area in which enemies are allowed to spawn
    private Vector2 SpawnArea()
    {
        Vector2 spawnPosition = new Vector2(Random.Range(spawnArea.xMin, spawnArea.xMax),  Random.Range(spawnArea.yMin, spawnArea.yMax));
        while ((spawnPosition - (Vector2)playerController.transform.position).magnitude < playerRange) {
            spawnPosition = new Vector2(Random.Range(spawnArea.xMin, spawnArea.xMax),  Random.Range(spawnArea.yMin, spawnArea.yMax));
        }
        return spawnPosition;
    }
    
    //Methods and Coroutines to spawn a set amount of a certain amount
    public void SpawnSkullsMethod()
    {
        StartCoroutine(SpawnSkulls());
    }
    public void SpawnKnightsMethod()
    {
        StartCoroutine(SpawnKnights());
    }
    public void SpawnBossMethod()
    {
        StartCoroutine(SpawnBosses());
    }
    private IEnumerator SpawnSkulls()
    {
        for (int i = 0; i < spawnAmount; i++)
        {
            Vector2 spawnPosition = SpawnArea();
            GameObject skull = Instantiate(skullEnemy, spawnPosition, Quaternion.identity, enemyParent);
            EventBus<EnemySpawnEventData>.Publish(new EnemySpawnEventData(skull));
            yield return null;
        }
    }
    private IEnumerator SpawnKnights()
    {
        for (int i = 0; i < spawnAmount; i++)
        {
            Vector2 spawnPosition = SpawnArea();
            GameObject knight = Instantiate(knightEnemy, spawnPosition, Quaternion.identity, enemyParent);
            EventBus<EnemySpawnEventData>.Publish(new EnemySpawnEventData(knight));
            yield return null;
        }
    }

    private IEnumerator SpawnBosses()
    {
        for (int i = 0; i < spawnAmount; i++)
        {
            Vector2 spawnPosition = SpawnArea();
            if (GameObject.FindWithTag("Boss") == null) // only spawn if no other bosses exist
            {
                GameObject boss = Instantiate(bossEnemy, spawnPosition, Quaternion.identity, enemyParent);
                EventBus<EnemySpawnEventData>.Publish(new EnemySpawnEventData(boss));
            }
            yield return null;
        }
    }
    
    //This method spawns the waves
    public void SpawnEnemies(EnemyWaveData enemies) // make this a coroutine
    {
        foreach (GameObject enemy in enemies.enemyWave)
        {
            Vector2 spawnPosition = new Vector2(Random.Range(spawnArea.xMin, spawnArea.xMax),  Random.Range(spawnArea.yMin, spawnArea.yMax));
            while ((spawnPosition - (Vector2)playerController.transform.position).magnitude < playerRange) {
                spawnPosition = new Vector2(Random.Range(spawnArea.xMin, spawnArea.xMax),  Random.Range(spawnArea.yMin, spawnArea.yMax));
            }

            if (enemy.CompareTag("Boss"))
            {
                if (GameObject.FindWithTag("Boss") == null) // only spawn if no other bosses exist
                {
                    GameObject instantiatedEnemy = Instantiate(enemy, spawnPosition, Quaternion.identity, enemyParent);
                    EventBus<EnemySpawnEventData>.Publish(new EnemySpawnEventData(instantiatedEnemy));
                }
            }
            else
            {
                GameObject instantiatedEnemy = Instantiate(enemy, spawnPosition, Quaternion.identity, enemyParent);
                EventBus<EnemySpawnEventData>.Publish(new EnemySpawnEventData(instantiatedEnemy));
            }
            
        }
    }
    
    //Change the spawn amount for spawning individual enemies using inputfields
    public void SpawnAmount(string amount)
    {
        Int32.TryParse(amount, out spawnAmount);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(spawnArea.center, spawnArea.size);
        Gizmos.DrawSphere(playerController.transform.position, playerRange);
    }
}
