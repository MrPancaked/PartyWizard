using System;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    public DamageData damageData;

    [SerializeField] private MoveDamageableType _moveDamageableType;

    private void Start()
    {
        _moveDamageableType = GetComponent<MoveDamageableType>();
    }

    private void MoveDamageable(Vector3 direction)
    {
        _moveDamageableType.MoveDamagable();
    }
}
