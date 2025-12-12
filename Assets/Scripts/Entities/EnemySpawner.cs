using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using Player;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private float playerRange;
    [SerializeField] private GameObject skullEnemy;
    [SerializeField] private GameObject knightEnemy;
    [SerializeField] private Transform enemyParent;
    [SerializeField] private Rect spawnArea;

    private int spawnAmount = 1;
    private GameManager gameManager;
    
    private void Start()
    {
        gameManager = GameManager.Instance;
        gameManager.SpawnEnemiesEvent += SpawnEnemies;
    }

    private void OnDisable()
    {
        gameManager.SpawnEnemiesEvent -= SpawnEnemies;
    }

    public void SpawnSkullsMethod()
    {
        StartCoroutine(SpawnSkulls());
    }
    public void SpawnKnightsMethod()
    {
        StartCoroutine(SpawnKnights());
    }
    private IEnumerator SpawnSkulls()
    {
        for (int i = 0; i < spawnAmount; i++)
        {
            Vector2 spawnPosition = new Vector2(Random.Range(spawnArea.xMin, spawnArea.xMax),  Random.Range(spawnArea.yMin, spawnArea.yMax));
            while ((spawnPosition - (Vector2)playerController.transform.position).magnitude < playerRange) {
                spawnPosition = new Vector2(Random.Range(spawnArea.xMin, spawnArea.xMax),  Random.Range(spawnArea.yMin, spawnArea.yMax));
            }
            GameObject skull = Instantiate(skullEnemy, spawnPosition, Quaternion.identity, enemyParent);
            EventBus<EnemySpawnEventData>.Publish(new EnemySpawnEventData(skull));
            yield return null;
        }
    }
    private IEnumerator SpawnKnights()
    {
        for (int i = 0; i < spawnAmount; i++)
        {
            Vector2 spawnPosition = new Vector2(Random.Range(spawnArea.xMin, spawnArea.xMax),  Random.Range(spawnArea.yMin, spawnArea.yMax));
            while ((spawnPosition - (Vector2)playerController.transform.position).magnitude < playerRange) {
                spawnPosition = new Vector2(Random.Range(spawnArea.xMin, spawnArea.xMax),  Random.Range(spawnArea.yMin, spawnArea.yMax));
            }
            GameObject knight = Instantiate(knightEnemy, spawnPosition, Quaternion.identity, enemyParent);
            EventBus<EnemySpawnEventData>.Publish(new EnemySpawnEventData(knight));
            yield return null;
        }
    }

    private void SpawnEnemies(EnemyWaveData enemies)
    {
        foreach (GameObject enemy in enemies.enemyWave)
        {
            Vector2 spawnPosition = new Vector2(Random.Range(spawnArea.xMin, spawnArea.xMax),  Random.Range(spawnArea.yMin, spawnArea.yMax));
            while ((spawnPosition - (Vector2)playerController.transform.position).magnitude < playerRange) {
                spawnPosition = new Vector2(Random.Range(spawnArea.xMin, spawnArea.xMax),  Random.Range(spawnArea.yMin, spawnArea.yMax));
            }
            GameObject instantiatedEnemy = Instantiate(enemy, spawnPosition, Quaternion.identity, enemyParent);
            EventBus<EnemySpawnEventData>.Publish(new EnemySpawnEventData(instantiatedEnemy));
        }
    }

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
