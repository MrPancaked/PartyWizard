using UnityEngine;

public class ExtraHpUpgrade : Upgrade
{

    private void Start()
    {
        upgradeName = "Extra Hp Upgrade";
        description = $"Adds +{playerStatController.extraHpUpgrade} hp to your max hp";
    }
    public override void DoUpgrade()
    {
        PlayerStatController.Instance.extraHp += playerStatController.extraHpUpgrade;
        base.DoUpgrade();
        EventBus<ExtraHpUpgradeEventData>.PublishNoParam();
        Debug.Log($"{upgradeName}: Added +{playerStatController.extraHpUpgrade} hp to your max hp");
    }
}

