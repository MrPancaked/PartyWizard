using UnityEngine;

public class ListViewInventoryPresenter : InventoryPresenter
{
    // Prefab used to display each item in the inventory list.
    [SerializeField] private ItemPresenter itemPresenterPrefab;
    [SerializeField] private Item noItem;
    [SerializeField] private Item notAvailableItemSlot;

    // Parent transform under which item UI elements will be instantiated.
    public Transform listParent;

    // Populates the inventory list UI with sorted items.
    public override void PresentInventory()
    {
        Debug.Log("Present Inventory");
        // Clear any existing item UI elements.
        ClearList();

        // Get sorted items from the inventory.
        Item[] items = Inventory.Instance.Items;

        // Instantiate and present each item in the UI.
        for (int i = 0; i < 4; i++)
        {
            if (i < items.Length)
            {
                ItemPresenter itemPresenter = Instantiate<ItemPresenter>(itemPresenterPrefab);
                itemPresenter.PresentItem(items[i]);

                // Set the parent and scale for proper UI layout.
                itemPresenter.transform.SetParent(listParent);
                itemPresenter.transform.localScale = Vector3.one;
            }
            else if (i < Inventory.Instance.maxItems)
            {
                ItemPresenter itemPresenter = Instantiate<ItemPresenter>(itemPresenterPrefab);
                itemPresenter.PresentItem(noItem);

                // Set the parent and scale for proper UI layout.
                itemPresenter.transform.SetParent(listParent);
                itemPresenter.transform.localScale = Vector3.one;
            }
            else if (i >= Inventory.Instance.maxItems)
            {
                ItemPresenter itemPresenter = Instantiate<ItemPresenter>(itemPresenterPrefab);
                itemPresenter.PresentItem(notAvailableItemSlot);

                // Set the parent and scale for proper UI layout.
                itemPresenter.transform.SetParent(listParent);
                itemPresenter.transform.localScale = Vector3.one;
            }
        }
    }
    // Clears all child item UI elements from the list except the parent itself.
    private void ClearList()
    {
        foreach (Transform transform in listParent.GetComponentsInChildren<Transform>())
        {
            if (transform != listParent)
                Destroy(transform.gameObject);
        }
    }
}
