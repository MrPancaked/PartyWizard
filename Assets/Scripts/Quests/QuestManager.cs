using System;
using System.Collections.Generic;
using Quests.Quests;
using Unity.VisualScripting;
using UnityEngine;

namespace Quests
{
    /// <summary>
    /// Class manages quests on the questUI object
    /// Does not work properly if the PauseMenu is disabled in the dungeon scene since it requires the quest objects to be enabled on awake.
    /// This is a flaw in its current design, but I didn't have the time to figure out a better solution.
    /// </summary>
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