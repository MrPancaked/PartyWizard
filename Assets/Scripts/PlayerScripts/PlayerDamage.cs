using System;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    public event Action<DamageData> onHit;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            DamageData damageData = other.gameObject.GetComponent<Damageable>().damageData;
            onHit?.Invoke(damageData);
        }
    }
}
