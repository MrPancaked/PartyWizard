using UnityEngine;


namespace Quests
{
    [CreateAssetMenu(fileName = "UseItemQuestData", menuName = "Scriptable Objects/UseItemQuestData")]
    public class UseItemQuestData : QuestData
    {
        public ItemData toBeUsed;
    }
}