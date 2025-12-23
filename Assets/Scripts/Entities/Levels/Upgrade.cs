using UnityEngine;

public abstract class Upgrade : MonoBehaviour
{
    protected string upgradeName;
    protected string description;
    protected PlayerStatController playerStatController;

    private void OnEnable()
    {
        playerStatController = PlayerStatController.Instance;
    }
    public virtual void DoUpgrade()
    {
        EventBus<PlayerUpgradeEventData>.PublishNoParam();
    }
}
