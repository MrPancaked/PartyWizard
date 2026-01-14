using System;
using Player;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

/*
 * Gives knockback, in the direction of the hit, to an object when its HpController takes damage.
 */
public class Knockback : MonoBehaviour
{
    [SerializeField] private HpController hpController;
    private Rigidbody2D rb;
    private void OnEnable()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        hpController =  gameObject.GetComponent<HpController>();
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
        }
    }
}
