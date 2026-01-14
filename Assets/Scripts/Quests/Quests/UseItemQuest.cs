using System;
using UnityEngine;

namespace Quests.Quests
{
    /// <summary>
    /// Quest that tracks if a certain item has been used based on the attached UseItemQuestData scriptableobject
    /// </summary>
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
                if (useItemEvent.item.Id == toBeUsed.Id) // add to the slider if the right item is used
                {
                    questSlider.value++;
                }
            }
        }
    }
}