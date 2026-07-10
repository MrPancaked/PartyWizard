using UnityEngine;

public class SpellCastAmountUpgrade : Upgrade
{
    private void Start()
    {
        upgradeName = "Extra Spell Upgrade";
        description = $"Adds +{playerStatController.spellCastAmount} to the amount of spells per cast";
        UpdateUpgradeText();
    }
    public override void DoUpgrade()
    {
        playerStatController.spellCastAmount += playerStatController.spellCastAmountUpgrade;
        base.DoUpgrade(); //send event
        //potentially initialize the EventData in Start so it doesn't have to be created every time.
        EventBus<SpellCastAmountUpgradeEventData>.Publish(new SpellCastAmountUpgradeEventData(this, PlayerStatController.Instance.spellCastAmountUpgrade));
        Debug.Log($"{upgradeName}: Added +{playerStatController.spellCastAmount} to the amount of spells per cast");
    }
}
