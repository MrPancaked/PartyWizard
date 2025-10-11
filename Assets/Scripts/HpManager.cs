using UnityEditor.Experimental.Licensing;
using UnityEngine;

public class HpManager
{
    private int hp;
    private int maxHp;

    public HpManager(EntitySettings entitySettings)
    {
        this.hp = entitySettings.hp;
        this.maxHp = entitySettings.maxHp;
    }

    public void IncreaseHp(int hpIncrease)
    {
        hp += hpIncrease;
    }

    public void DecreaseHp(int hpDecrease)
    {
        hp -= hpDecrease;
    }
}
