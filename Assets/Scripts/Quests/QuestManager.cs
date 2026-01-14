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
        private List<Quest> questList;

        private void OnEnable()
        {
            EventBus<EnemyDieEventData>.OnEventPublished += UpdateQuests;
            EventBus<UseItemEvent>.OnEventPublished += UpdateQuests;
        }

        private void OnDisable()
        {
            EventBus<EnemyDieEventData>.OnEventPublished -= UpdateQuests;
            EventBus<UseItemEvent>.OnEventPublished -= UpdateQuests;
            GameManager.Instance.PauseEvent -= InitializeQuestUI;
        }

        private void Awake()
        {
            questList = new List<Quest>();
            foreach (Quest quest in questUI.GetComponentsInChildren<Quest>())
            {
                questList.Add(quest);
            }
            InitializeQuestUI();
        }

        private void Start()
        {
            
            GameManager.Instance.PauseEvent += InitializeQuestUI;
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
                    case UseItemQuest useItemQuest:
                        useItemQuest.image.sprite = useItemQuest.toBeUsed.itemIcon;
                        break;
                }
            }
        }

        private void UpdateQuests(EventData eventData)
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
                    case UseItemQuest useItemQuest:
                        if (eventData is UseItemEvent useItemEvent)
                        {
                            useItemQuest.UpdateSlider(useItemEvent);
                        }
                        break;
                }
            }
        }
    }
}