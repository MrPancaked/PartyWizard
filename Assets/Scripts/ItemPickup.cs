using System;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public static Action<Item> itemPickupEvent;
    public static Action xpPickupEvent;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject triggerObj = collision.gameObject;
        if (triggerObj.CompareTag("XP"))
        {
            xpPickupEvent?.Invoke();
            Destroy(triggerObj.gameObject);
        }
        else if (triggerObj.CompareTag("Item"))
        {
            Item item = triggerObj.GetComponent<ItemHolder>().GiveItem();
            itemPickupEvent?.Invoke(item);
            Destroy(triggerObj.gameObject);
        }
    }
}
