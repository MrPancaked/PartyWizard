using UnityEngine;

public class ExtraHpUpgrade : Upgrade
{

    private void Start()
    {
        upgradeName = "Extra Hp";
        description = $"Adds +{playerStatController.extraHpUpgrade} hp to your max hp";
        UpdateUpgradeText();
    }
    public override void DoUpgrade()
    {
        playerStatController.extraHp += playerStatController.extraHpUpgrade;
        base.DoUpgrade(); //send event
        EventBus<ExtraHpUpgradeEventData>.PublishNoParam();
        Debug.Log($"{upgradeName}: Added +{playerStatController.extraHpUpgrade} hp to your max hp");
    }
}

