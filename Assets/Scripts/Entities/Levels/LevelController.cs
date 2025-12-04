using System;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public XpData xpData;
    private int level;
    
    public Action<int> LevelUpEvent;
    public Action<int> XPEvent;

    private void OnEnable()
    {
        
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject triggerObj = collision.gameObject;
        if (triggerObj.CompareTag("XP"))
        {
            xpData.xpCount += xpData.xpMultiplier;
            Destroy(triggerObj.gameObject);

            if (xpData.xpCount >= xpData.xpForLevel)
            {
                xpData.xpCount = 0;
                LevelUp();
            }
        }
    }
    private void LevelUp()
    {
        Debug.Log("LevelUp");
        level++;
    }
}
