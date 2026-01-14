using System;
using UnityEngine;
//ItemData is basically a reference to an instance of an Item using the CreateItem() method
[CreateAssetMenu(fileName = "ItemData", menuName = "Scriptable Objects/ItemData")]
public class ItemData : ScriptableObject
{
    [Header("Unique id for each item")]
    public string id;

    [Header("Core properties")]
    public string itemName;

    [Header("Visuals")]
    public Sprite itemIcon;
    public GameObject itemModel;

    public Item CreateItem()
    {
        return new Item(this);
    }
}

[Serializable]
public class Item
{
    [Header("Unique id for each item")]
    [SerializeField]
    private string id;
    public string Id => id;//This setup allows access to the private field 'id'
    //while also allows it to be shown in the inspector

    [Header("Core properties")]
    [SerializeField]
    private string itemName;
    public string ItemName => itemName;

    [Header("Visuals")]
    public Sprite itemIcon;
    public GameObject itemModel;

    public Item(ItemData itemData)
    {
        id = itemData.id;
        itemName = itemData.itemName;

        itemIcon = itemData.itemIcon;
        itemModel = itemData.itemModel;
    }
}
