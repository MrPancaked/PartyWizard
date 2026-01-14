using UnityEngine;

public class FlatDamageUpgrade : Upgrade
{
    private void Start()
    {
        upgradeName = "Flat Damage";
        description = $"Adds +{playerStatController.flatDamageUpgrade} damage to your attacks and spells";
        UpdateUpgradeText();
    }
    public override void DoUpgrade()
    {
        playerStatController.flatDamage += playerStatController.flatDamageUpgrade;
        base.DoUpgrade();
        Debug.Log($"{upgradeName}: Added +{playerStatController.flatDamageUpgrade} damage to your attacks and spells");
    }
}
