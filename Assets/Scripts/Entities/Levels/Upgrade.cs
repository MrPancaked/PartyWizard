using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class Upgrade : MonoBehaviour
{
    protected string upgradeName;
    protected string description;
    [SerializeField] protected TextMeshProUGUI nameText;
    [SerializeField] protected TextMeshProUGUI descriptionText;
    [SerializeField] protected Image iconImage;
    [SerializeField] protected Sprite upgradeIcon;
    protected PlayerStatController playerStatController;

    private void OnEnable()
    {
        playerStatController = PlayerStatController.Instance;
    }

    protected void UpdateUpgradeText()
    {
        if (nameText != null) nameText.text = upgradeName;
        if (descriptionText != null) descriptionText.text = description;
        if (iconImage != null && upgradeIcon != null) iconImage.sprite = upgradeIcon;
    }

    public virtual void DoUpgrade()
    {
        EventBus<PlayerUpgradeEventData>.PublishNoParam();
    }
}
