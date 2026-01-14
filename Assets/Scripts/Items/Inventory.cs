using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
    {
        public static Inventory Instance { get; private set; }

        public int maxItems;
        // List of item data assets used to generate actual items at runtime.
        [SerializeField]
        private List<ItemData> itemDatas;

        // List of instantiated items currently in the inventory.
        [SerializeReference]
        private List<Item> items;

        // Public read-only property to access a copy of the items list.
        public Item[] Items => items.ToArray();
        
        public static Action presentInventoryEvent;

        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; } // Singleton
            Instance = this;
            
            GenerateInventory();// Create items based on itemDatas.
        }

        private void OnEnable()
        {
            ItemPickup.itemPickupEvent += AddItem;
        }

        private void OnDisable()
        {
            ItemPickup.itemPickupEvent -= AddItem;
        }

        // Instantiates items based on the item data list.
        private void GenerateInventory()
        {
            items = new List<Item>();
            foreach (ItemData itemData in itemDatas)
            {
                items.Add(itemData.CreateItem()); // Create an item from its data.
            }
        }

        // Adds an item to the inventory.
        public void AddItem(Item item)
        {
            items.Add(item);
            presentInventoryEvent?.Invoke();
            Debug.Log($"{item.ItemName} added to inventory");
        }

        // Removes an item from the inventory.
        public void RemoveItem(Item item)
        {
            //Normally I would make a separate method that invokes this event when an item gets used
            //but now the only gets called when and item gets used so I'm just calling it here.
            EventBus<UseItemEvent>.Publish(new UseItemEvent(item));
            items.Remove(item);
            presentInventoryEvent?.Invoke();
        }

        public void AddItemSlot()
        {
            if (maxItems < 4) maxItems++;
            presentInventoryEvent?.Invoke();
        }
    }
