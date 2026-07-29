using Player;
using Projectiles;
using UnityEngine;

public class SpellUpgrade : Upgrade
{
    [SerializeField] private ProjectileController spellPrefab;
    private void Start()
    {
        upgradeName = "Spell Upgrade";
        description = $"Gives the player a different spell but sets all stats back to the starting stats";
        UpdateUpgradeText();
    }
    public override void DoUpgrade()
    {
        EventBus<SpellUpgradeEventData>.Publish(new SpellUpgradeEventData(this , spellPrefab));
        EventBus<SpellUpgradeEventData>.PublishNoParam();
        playerStatController.InitializeStats();
        base.DoUpgrade();
        Debug.Log($"{upgradeName}: Gave player a different spell but sets all stats back to the starting stats");
    }
}
