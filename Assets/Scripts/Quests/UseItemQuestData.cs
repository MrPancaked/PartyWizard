using UnityEngine;


namespace Quests
{
    /// <summary>
    /// Holds the ItemData Scriptable object that needs to match the used item's id to progress the quest
    /// </summary>
    [CreateAssetMenu(fileName = "UseItemQuestData", menuName = "Scriptable Objects/UseItemQuestData")]
    public class UseItemQuestData : QuestData
    {
        public ItemData toBeUsed;
    }
}