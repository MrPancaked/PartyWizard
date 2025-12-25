using System;
using TMPro;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using Random = UnityEngine.Random;

[RequireComponent(typeof(ItemPickup))]
public class LevelController : MonoBehaviour
{
    public XpData xpData;
    [SerializeField] private TextMeshProUGUI levelCounter;
    
    public int xpCount{ get; private set; }
    public int level { get; private set; }
    
    public Action LevelUpEvent;
    public Action XpEvent;
    
    private void Start()
    {
        if (xpData.xpCount > xpData.xpForLevel) xpData.xpCount = xpData.xpForLevel;
        xpCount =  xpData.xpCount;
    }

    private void OnEnable()
    {
        ItemPickup.xpPickupEvent += GainXp;
    }

    private void OnDisable()
    {
        ItemPickup.xpPickupEvent -= GainXp;
    }

    public void GainXp() //public for button
    {
        int xpGain = xpData.xpMultiplier;
        xpCount += xpGain;
        XpEvent?.Invoke();
        if (xpCount >= xpData.xpForLevel)
        {
            LevelUp();
        }
    }
    public void LevelUp() // public for button
    {
        level++;
        if (levelCounter != null) levelCounter.text = $"{level}";
        xpCount = 0;
        LevelUpEvent?.Invoke();
    }
}
