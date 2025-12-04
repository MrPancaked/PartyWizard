using System;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public XpData xpData;
    public int xpCount{ get; private set; }
    public int level { get; private set; }
    
    public Action LevelUpEvent;
    public Action<int> XPEvent;

    private void Start()
    {
        if (xpData.xpCount > xpData.xpForLevel) xpData.xpCount = xpData.xpForLevel;
        xpCount =  xpData.xpCount;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject triggerObj = collision.gameObject;
        if (triggerObj.CompareTag("XP"))
        {
            GainXp();
            Destroy(triggerObj.gameObject);
        }
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
    private void LevelUp()
    {
        LevelUpEvent?.Invoke();
        Debug.Log("LevelUp");
        level++;
    }
}
