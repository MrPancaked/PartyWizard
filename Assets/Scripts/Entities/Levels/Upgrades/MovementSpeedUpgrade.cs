using UnityEngine;

public class MovementSpeedUpgrade : Upgrade
{
    private void Start()
    {
        upgradeName = "Movement Speed Upgrade";
        description = $"Adds +{playerStatController.movementSpeedUpgrade} to your movement speed";
        UpdateUpgradeText();
    }
    public override void DoUpgrade()
    {
        playerStatController.movementSpeed += playerStatController.movementSpeedUpgrade;
        base.DoUpgrade(); //send event
        //potentially initialize the MovementSpeedUpgradeEventData in Start so it doesn't have to be created every time.
        EventBus<MovementSpeedUpgradeEventData>.Publish(new MovementSpeedUpgradeEventData(this, PlayerStatController.Instance.movementSpeedUpgrade));
        Debug.Log($"{upgradeName}: Added +{playerStatController.movementSpeedUpgrade} to your movement speed");
    }
}
