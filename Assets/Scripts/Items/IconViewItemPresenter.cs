using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Presents an Item icon in a UI image
/// </summary>
public class IconViewItemPresenter : ItemPresenter
{
    [SerializeField] private Image icon;

    public override void PresentItem(Item item)
    {
        icon.sprite = item.itemIcon;
    }

    public void DisplayItemInfo()
    {
        //todo: display the item info by modifying ItemInfoDisplayer.itemInfo
    }
}
