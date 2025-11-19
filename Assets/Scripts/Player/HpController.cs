
using UnityEngine;
using System;
using Projectiles;
using ScriptableObjects.Player;

namespace Player
{
    public class HpController : MonoBehaviour
    {
        public event Action<int> TakeDamageEvent;
        
        private int hp;
        private int shield;
        private int contactDamage;
        public ScriptableObjects.Player.HpData hpData; //public so playercontroller can update the controller data classes
    
        private void Start()
        {
            Initialize();
        }

        private void OnEnable()
        {
            TakeDamageEvent += TakeDamage;
        }

        private void OnDisable()
        {
            TakeDamageEvent -= TakeDamage;
        }

        public void Initialize()
        {
            hp = hpData.startHp;
            shield = hpData.startShield;
            contactDamage = hpData.contactDamage;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            GameObject collidedObj = collision.gameObject;
            if (gameObject.CompareTag("Player"))
            {
                if (collidedObj.CompareTag("Enemy"))
                {
                    int receivedDamage = collidedObj.GetComponent<HpController>().contactDamage;
                    TakeDamageEvent?.Invoke(receivedDamage);
                }
                else if (collidedObj.CompareTag("Projectile"))
                {
                    SpellData spellData = collidedObj.GetComponent<ProjectileController>().spellData;
                    if (spellData.hurtPlayer)
                    {
                        int receivedDamage = spellData.damage;
                        if (spellData.aoeEffect) receivedDamage += spellData.aoeDamage;
                        TakeDamageEvent?.Invoke(receivedDamage);
                        Destroy(collidedObj);
                    }
                }
            }
            else if (gameObject.CompareTag("Enemy"))
            {
                if (collidedObj.CompareTag("Player"))
                {
                    int receivedDamage = collidedObj.GetComponent<HpController>().contactDamage;
                    TakeDamageEvent?.Invoke(receivedDamage);
                }
                else if (collidedObj.CompareTag("Projectile"))
                {
                    SpellData spellData = collidedObj.GetComponent<ProjectileController>().spellData;
                    if (spellData.hurtEnemy)
                    {
                        int receivedDamage = spellData.damage;
                        if (spellData.aoeEffect) receivedDamage += spellData.aoeDamage;
                        TakeDamageEvent?.Invoke(receivedDamage);
                        Destroy(collidedObj);
                    }
                }
            }
        }
        
        private void TakeDamage(int damage)
        {
            Debug.Log($"taken damage: {damage}");
        }
    }
}
