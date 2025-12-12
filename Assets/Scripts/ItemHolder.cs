using System;
using UnityEngine;

public class ItemHolder : MonoBehaviour
{
    [SerializeField] private ItemData itemData;
    public Item GiveItem()
    {
        Item item = itemData.CreateItem();
        return item;
    }
}
