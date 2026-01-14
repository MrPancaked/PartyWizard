using UnityEngine;
/// <summary>
/// scriptable object that holds an array of enemies that can be used to spawn waves or sets of enemies in the EnemySpawner
/// </summary>
[CreateAssetMenu(fileName = "EnemyWaveData", menuName = "Scriptable Objects/EnemyWaveData")]
public class EnemyWaveData : ScriptableObject
{
    public GameObject[] enemyWave;
}
