using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Quests
{
    public class QuestManager : MonoBehaviour
    {
        [SerializeField] private GameObject questUI;
        [SerializeField] private List<QuestData> startQuestList;
        private List<KillQuest> killQuestList;
        private List<UseItemQuest> useItemQuestList;
        private List<GatherItemQuest> gatherItemQuestList;

        private void Awake()
        {
            EventBus<EnemyDieEventData>.OnEventPublished += UpdateKillQuests;

            foreach (QuestData questData in startQuestList)
            {
                switch (questData)
                {
                    case KillQuestData killQuestData:
                        break;
                }
            }
        }

        private void UpdateKillQuests(EnemyDieEventData enemyDieEventData)
        {
            foreach (var quest in killQuestList)
            {
                if (quest.GetType() == typeof(KillQuest))
                {
                    
                }
            }
        }
    }
    public abstract class Quest
    {
        public string questName;
        public string description;
        public int amount;
    }

    public class KillQuest : Quest
    {
        public GameObject toBeKilled;
    }

    public class UseItemQuest : Quest
    {
        public Item item;
    }

    public class GatherItemQuest : Quest
    {
        public Item item;
    }
}