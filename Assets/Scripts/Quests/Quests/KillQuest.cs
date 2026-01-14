using System;
using UnityEngine;

namespace Quests.Quests
{
    /// <summary>
    /// Quest that tracks if a certain enemy has been killed based on the attached KillQuestData scriptableobject
    /// </summary>
    public class KillQuest : Quest
    {
        [HideInInspector] public GameObject toBeKilled;
        [SerializeField] protected KillQuestData killQuestData;

        private void Awake()
        {
            InitializeKillQuest(killQuestData);
        }
        protected void InitializeKillQuest(KillQuestData pKillQuestData)
        {
            questName = pKillQuestData.questName;
            description = pKillQuestData.description;
            amount = pKillQuestData.amount;
            toBeKilled = pKillQuestData.toBeKilled;
        }

        public override void UpdateSlider(EventData eventData)
        {
            if (eventData is EnemyDieEventData enemyDieEventData)
            {
                if (toBeKilled.CompareTag(enemyDieEventData.enemyObject.tag)) //add to slider value if the right enemy is killed
                {
                    questSlider.value++;
                }
            }
        }
    }
}