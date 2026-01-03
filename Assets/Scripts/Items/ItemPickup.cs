using System;
using FMODUnity;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public static Action<Item> itemPickupEvent;
    public static Action xpPickupEvent;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject triggerObj = collision.gameObject;
        if (triggerObj.CompareTag("Xp"))
        {
            AudioManager.Instance.PlayOneShot(FMODEvents.Instance.pickupSound, gameObject.transform.position);
            xpPickupEvent?.Invoke();
            Destroy(triggerObj.gameObject);
        }
        else if (triggerObj.CompareTag("Item"))
        {
            Item item = triggerObj.GetComponent<ItemHolder>().GiveItem();
            if (Inventory.Instance.Items.Length < Inventory.Instance.maxItems)
            {
                itemPickupEvent?.Invoke(item);
                Destroy(triggerObj.gameObject);
            }
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        OnTriggerEnter2D(other);
    }
}
