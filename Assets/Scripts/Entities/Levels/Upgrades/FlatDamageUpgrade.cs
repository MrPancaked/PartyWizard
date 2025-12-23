using UnityEngine;

public class FlatDamageUpgrade : Upgrade
{
    private void Start()
    {
        upgradeName = "Flat Damage Upgrade";
        description = $"Adds +{playerStatController.flatDamageUpgrade} damage to your attacks and spells";
    }
    public override void DoUpgrade()
    {
        playerStatController.flatDamage += playerStatController.flatDamageUpgrade;
        base.DoUpgrade();
        //EventBus<FlatDamageUpgradeEventData>.PublishNoParam();
        Debug.Log($"{upgradeName}: Added +{playerStatController.flatDamageUpgrade} damage to your attacks and spells");
    }
}
