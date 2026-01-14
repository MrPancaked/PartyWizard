using System;
using System.Collections;
using Player;
using UnityEditor;
using UnityEngine;

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
        if (Inventory.Instance.Items.Length < Inventory.Instance.maxItems) layerMask |= LayerMask.GetMask("Item");
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
        if (Inventory.Instance.Items.Length < Inventory.Instance.maxItems) layerMask |= LayerMask.GetMask("Item");
        Collider2D[] colliders = Physics2D.OverlapBoxAll(new Vector2(0,0), new Vector2(14f, 14f), 0, layerMask);
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
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, range);
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(14,14,14));
    }
}
