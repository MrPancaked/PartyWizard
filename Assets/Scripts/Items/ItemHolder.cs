using System;
using UnityEngine;

/// <summary>
/// Gameobjects can have this class attached to carry the Items information so the player can pick up the right item later.
/// </summary>
public class ItemHolder : MonoBehaviour
{
    [SerializeField] private ItemData itemData;
    public Item GiveItem()
    {
        Item item = itemData.CreateItem();
        return item;
    }
}
