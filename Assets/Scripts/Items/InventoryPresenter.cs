using System;
using UnityEngine;

/// <summary>
/// Base class for different inventory presenters
/// </summary>
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

    private void Start()
    {
        PresentInventory();
    }

    public abstract void PresentInventory();
}
