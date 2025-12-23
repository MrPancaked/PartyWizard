using UnityEngine;

public class DamageMultiplierUpgrade : Upgrade
{
    private void Start()
    {
        upgradeName = "Damage Multiplier";
        description = $"Adds +{playerStatController.damageMultiplierUpgrade} to your damage multiplier";
        UpdateUpgradeText();
    }
    public override void DoUpgrade()
    {
        playerStatController.damageMultiplier += playerStatController.damageMultiplierUpgrade;
        base.DoUpgrade();
        //EventBus<DamageMultiplierUpgradeEventData>.PublishNoParam();
        Debug.Log($"{upgradeName}: Added +{playerStatController.damageMultiplierUpgrade} to your damage multiplier");
    }
}
