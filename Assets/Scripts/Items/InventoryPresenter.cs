using System;
using UnityEngine;

public abstract class InventoryPresenter : MonoBehaviour
{
    protected virtual void OnEnable()
    {
        Inventory.presentInventoryEvent += PresentInventory;
    }
    protected virtual void OnDisable()
    {
        Inventory.presentInventoryEvent -= PresentInventory;
    }
    public abstract void PresentInventory();
}
