using System;
using UnityEngine;

public abstract class InventoryPresenter : MonoBehaviour
{
    protected virtual void OnEnable()
    {
        Inventory.itemAddEvent += PresentInventory;
    }
    protected virtual void OnDisable()
    {
        Inventory.itemAddEvent -= PresentInventory;
    }
    public abstract void PresentInventory();
}
