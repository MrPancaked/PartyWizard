using System;
using System.Collections.Generic;
using Quests.Quests;
using Unity.VisualScripting;
using UnityEngine;

namespace Quests
{
    public class QuestManager : MonoBehaviour
    {
        [SerializeField] private GameObject questUI;
        [SerializeField] private GameObject KillQuestPrefab;
        [SerializeField] private GameObject UseItemQuestPrefab;
        [SerializeField] private List<Quest> questList;

        private void OnEnable()
        {
            EventBus<EnemyDieEventData>.OnEventPublished += UpdateKillQuests;
        }

        private void OnDisable()
        {
            EventBus<EnemyDieEventData>.OnEventPublished -= UpdateKillQuests;
        }

        private void InitializeQuest()
        {
            
        }

        private void Start()
        {
            InitializeQuestUI();
        }

        private void InitializeQuestUI()
        {
            foreach (Quest quest in questList)
            {
                quest.questSlider.maxValue = quest.amount;
                quest.questDescriptionText.text = quest.description;
                switch (quest)
                {
                    case KillQuest killQuest:
                        killQuest.image.sprite = killQuest.toBeKilled.GetComponent<SpriteRenderer>().sprite;
                        break;
                }
            }
        }

        private void UpdateKillQuests(EventData eventData)
        {
            foreach (Quest quest in questList)
            {
                switch (quest)
                {
                    case KillQuest killQuest:
                        if (eventData is EnemyDieEventData enemyDieEventData)
                        {
                            killQuest.UpdateSlider(enemyDieEventData);
                        }
                        break;
                }
            }
        }
    }
}