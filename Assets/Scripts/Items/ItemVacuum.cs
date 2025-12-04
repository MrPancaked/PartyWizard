using System;
using Player;
using UnityEditor;
using UnityEngine;

public class ItemVacuum : MonoBehaviour
{
    [SerializeField] float range;
    [SerializeField] float acceleration;

    private void FixedUpdate()
    {
        SuckXp();
    }
    private void SuckXp()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, range, LayerMask.GetMask("Item"));
        foreach (Collider2D hit in colliders)
        {
            Rigidbody2D hitRb = hit.GetComponent<Rigidbody2D>();
            if (hitRb != null)
            {
                Vector2 hitRbPos = hitRb.transform.position;
                float distance = (transform.position - hitRb.transform.position).magnitude;
                Vector2 direction = ((Vector2)transform.position - hitRbPos).normalized;
                hitRb.AddForce((1f - distance / range) * acceleration * direction, ForceMode2D.Impulse);
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
