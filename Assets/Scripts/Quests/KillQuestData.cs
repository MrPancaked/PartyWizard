using UnityEngine;
/// <summary>
/// Holds the enemy object that needs to be killed for a certain quest
/// </summary>
[CreateAssetMenu(fileName = "KillQuestData", menuName = "Scriptable Objects/KillQuestData")]
public class KillQuestData : QuestData
{
    public GameObject toBeKilled;
}
