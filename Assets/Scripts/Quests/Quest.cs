using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Quests
{
    /// <summary>
    /// Base class for different kinds of quests
    /// uses QuestData scriptable objects for its values
    /// updates sliders upon game event calls in the quest manager
    /// </summary>
    public abstract class Quest : MonoBehaviour
    {
        public Image image;
        public TextMeshProUGUI questNameText;
        public TextMeshProUGUI questDescriptionText;
        [HideInInspector] public string questName;
        [HideInInspector] public string description;
        [HideInInspector] public int amount;

        [SerializeField] public Slider questSlider;
        public abstract void UpdateSlider(EventData eventData);
    }
}