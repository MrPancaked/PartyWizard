using System;
using System.Collections;
using Player;
using UnityEditor;
using UnityEngine;

/*
 * Makes Items in the Scene gravitate towards the player in a certain range with a certain acceleration and range.
 */
public class ItemVacuum : MonoBehaviour
{
    [SerializeField] float range;
    [SerializeField] float acceleration;
    private bool clearItems = false;
    
    private GameManager gameManager;

    private void Start()
    {
        if (GameManager.Instance != null)
        {
            gameManager = GameManager.Instance;
            gameManager.RoomClearedEvent += SetClearItemsTrue;
            gameManager.RoomStartEvent += SetClearItemsFalse;
        }
        

        clearItems = false;
    }

    private void OnDisable()
    {
        if (gameManager != null)
        {
            gameManager.RoomClearedEvent -= SetClearItemsTrue;
            gameManager.RoomStartEvent -= SetClearItemsFalse;
        }
    }
    private void FixedUpdate()
    {
        if (!clearItems) SuckItems();
        else if (clearItems) ClearItems();
    }
    private void SuckItems()
    {
        LayerMask layerMask = LayerMask.GetMask("Xp");
        if (Inventory.Instance.Items.Length < Inventory.Instance.maxItems) // if inventory fits more items add item layer to the layermask
            layerMask |= LayerMask.GetMask("Item"); 
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, range, layerMask);
        foreach (Collider2D hit in colliders)
        {
            Rigidbody2D hitRb = hit.GetComponent<Rigidbody2D>();
            if (hitRb != null)
            {
                Vector2 hitRbPos = hitRb.transform.position;
                float distance = ((Vector2)transform.position - hitRbPos).magnitude;
                Vector2 direction = ((Vector2)transform.position - hitRbPos).normalized;
                hitRb.AddForce((1f - distance / (range + 1)) * acceleration * direction, ForceMode2D.Impulse); //+1 to range to stop objects from moving away on edge cases where the hitbox of the objects is in range but the centre is still out of range
            }
        }
    }

    //three methods used to make all items in the room gravitate when the room is cleared
    private void SetClearItemsTrue()
    {
        clearItems = true;
    }
    private void SetClearItemsFalse()
    {
        clearItems = false;
    }
    private void ClearItems()
    {
        LayerMask layerMask = LayerMask.GetMask("Xp");
        if (Inventory.Instance.Items.Length < Inventory.Instance.maxItems) // if inventory fits more items add item layer to the layermask
            layerMask |= LayerMask.GetMask("Item");
        Collider2D[] colliders = Physics2D.OverlapBoxAll(new Vector2(0,0), new Vector2(14f, 14f), 0, layerMask);//OverlapBox the size of the room
        foreach (Collider2D hit in colliders)
        {
            Rigidbody2D hitRb = hit.GetComponent<Rigidbody2D>();
            if (hitRb != null)
            {
                Vector2 hitRbPos = hitRb.transform.position;
                Vector2 direction = ((Vector2)transform.position - hitRbPos).normalized;
                hitRb.AddForce(acceleration * direction, ForceMode2D.Impulse);
            }
        }
    }
    
    //visualising the areas in which items gravitate towards the player
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        if (!clearItems)
            Gizmos.DrawWireSphere(transform.position, range);
        if (clearItems)
            Gizmos.DrawWireCube(Vector3.zero, new Vector3(14,14,14));
    }
}
