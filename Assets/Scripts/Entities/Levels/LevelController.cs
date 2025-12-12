using System;
using TMPro;
using UnityEngine;
[RequireComponent(typeof(ItemPickup))]
public class LevelController : MonoBehaviour
{
    public XpData xpData;
    [SerializeField] private TextMeshProUGUI levelCounter;
    public int xpCount{ get; private set; }
    public int level { get; private set; }
    
    public Action LevelUpEvent;
    public Action<int> XPEvent;
    
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
        XPEvent?.Invoke(xpGain);
        if (xpCount >= xpData.xpForLevel)
        {
            xpCount = 0;
            LevelUp();
        }
    }
    public void LevelUp() // public for button
    {
        LevelUpEvent?.Invoke();
        level++;
        levelCounter.text = $"{level}";
    }
}
