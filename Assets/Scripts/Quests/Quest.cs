using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Quests
{
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