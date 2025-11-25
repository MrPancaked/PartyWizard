
using UnityEngine;
using System;
using Projectiles;
using ScriptableObjects.Player;

namespace Player
{
    public class HpController : MonoBehaviour
    {
        public event Action<int> TakeDamageEvent;
        
        public int hp {get; private set;}
        public int shield {get; private set;}
        public int contactDamage {get; private set;}
        public bool takeDamage {get; private set;}
        public ScriptableObjects.Player.HpData hpData; //public so playercontroller can update the controller data classes
    
        private void Awake()
        {
            Initialize();
        }

        public void Initialize()
        {
            if (hpData.startHp > hpData.maxHp) hpData.startHp = hpData.maxHp;
            hp = hpData.startHp;
            shield = hpData.startShield;
            contactDamage = hpData.contactDamage;
            takeDamage = hpData.takeDamage;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            GameObject collidedObj = collision.gameObject;
            if (gameObject.CompareTag("Player"))
            {
                if (collidedObj.CompareTag("Enemy"))
                {
                    int receivedDamage = collidedObj.GetComponent<HpController>().contactDamage;
                    
                    TakeDamage(receivedDamage);
                    
                }
                else if (collidedObj.CompareTag("Projectile"))
                {
                    SpellData spellData = collidedObj.GetComponent<ProjectileController>().spellData;
                    if (spellData.hurtPlayer)
                    {
                        int receivedDamage = spellData.damage;
                        TakeDamage(receivedDamage);
                        Destroy(collidedObj);
                    }
                }
            }
            else if (gameObject.CompareTag("Enemy"))
            {
                if (collidedObj.CompareTag("Player"))
                {
                    int receivedDamage = collidedObj.GetComponent<HpController>().contactDamage;
                    
                    TakeDamage(receivedDamage);
                }
                else if (collidedObj.CompareTag("Projectile"))
                {
                    SpellData spellData = collidedObj.GetComponent<ProjectileController>().spellData;
                    if (spellData.hurtEnemy)
                    {
                        int receivedDamage = spellData.damage;
                        TakeDamage(receivedDamage);
                        Destroy(collidedObj);
                    }
                }
            }
        }
        
        public void TakeDamage(int damage)
        {
            if (takeDamage)
            {
                shield -= damage;
                if (shield < 0)
                {
                    hp -= -shield;
                    shield = 0;
                    if (hp < 0) hp = 0;
                }
                Debug.Log($"{gameObject.name} has taken damage: {damage}, current hp: {hp}");
                TakeDamageEvent?.Invoke(damage);
                if (hp == 0)
                {
                    Die();
                }
            }
        }
        private void Die()
        {
            Destroy(gameObject);
        }
    }
}
