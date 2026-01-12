using System;
using UnityEngine;

namespace Quests.Quests
{
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

        protected void Start()
        {
            //image.sprite = toBeKilled.GetComponent<SpriteRenderer>().sprite;
        }

        public override void UpdateSlider(EventData eventData)
        {
            if (eventData is EnemyDieEventData enemyDieEventData)
            {
                if (toBeKilled.CompareTag(enemyDieEventData.enemyObject.tag))
                {
                    questSlider.value++;
                }
            }
        }
    }
}