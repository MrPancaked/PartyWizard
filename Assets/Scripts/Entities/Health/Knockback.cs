using System;
using Player;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Knockback : MonoBehaviour
{
    [SerializeField] private HpController hpController;
    private Rigidbody2D rb;
    private void OnEnable()
    {
        rb = GetComponent<Rigidbody2D>();
        hpController.TakeDamageEvent += TakeKnockBack;
    }

    private void OnDisable()
    {
        hpController.TakeDamageEvent -= TakeKnockBack;
    }

    private void TakeKnockBack(TakeDamageData takeDamageData)
    {
        Vector2 hitDirection = (takeDamageData.hitLocation - (Vector2)hpController.transform.position).normalized;
        if (rb != null)
        {
            rb.AddForce(-hitDirection * takeDamageData.power, ForceMode2D.Impulse);
            Debug.Log($"knockback: {-hitDirection},  power: {takeDamageData.power}");
        }
    }
}
