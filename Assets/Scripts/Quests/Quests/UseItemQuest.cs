using System;
using UnityEngine;

namespace Quests.Quests
{
    public class UseItemQuest : Quest
    {
        [HideInInspector] public Item toBeUsed;
        [SerializeField] protected UseItemQuestData useItemQuestData;

        private void Awake()
        {
            InitializeUseItemQuest(useItemQuestData);
        }
        protected void InitializeUseItemQuest(UseItemQuestData pUseItemQuestData)
        {
            questName = pUseItemQuestData.questName;
            description = pUseItemQuestData.description;
            amount = pUseItemQuestData.amount;
            toBeUsed = pUseItemQuestData.toBeUsed.CreateItem();
        }

        public override void UpdateSlider(EventData eventData)
        {
            if (eventData is UseItemEvent useItemEvent)
            {
                if (useItemEvent.item.Id == toBeUsed.Id)
                {
                    questSlider.value++;
                }
            }
        }
    }
}