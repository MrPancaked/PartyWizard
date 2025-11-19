
using UnityEngine;
using System;
using Projectiles;
using ScriptableObjects.Player;

namespace Player
{
    public class HpController : MonoBehaviour
    {
        public event Action TakeDamageEvent;
        
        public int hp {get; private set;}
        public int shield {get; private set;}
        public int contactDamage {get; private set;}
        public bool takeDamage {get; private set;}
        public ScriptableObjects.Player.HpData hpData; //public so playercontroller can update the controller data classes
    
        private void Start()
        {
            Initialize();
        }

        public void Initialize()
        {
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
                    TakeDamageEvent?.Invoke();
                }
                else if (collidedObj.CompareTag("Projectile"))
                {
                    SpellData spellData = collidedObj.GetComponent<ProjectileController>().spellData;
                    if (spellData.hurtPlayer)
                    {
                        int receivedDamage = spellData.damage;
                        if (spellData.aoeEffect) receivedDamage += spellData.aoeDamage;
                        
                        TakeDamage(receivedDamage);
                        TakeDamageEvent?.Invoke();
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
                    TakeDamageEvent?.Invoke();
                }
                else if (collidedObj.CompareTag("Projectile"))
                {
                    SpellData spellData = collidedObj.GetComponent<ProjectileController>().spellData;
                    if (spellData.hurtEnemy)
                    {
                        int receivedDamage = spellData.damage;
                        if (spellData.aoeEffect) receivedDamage += spellData.aoeDamage;
                        
                        TakeDamage(receivedDamage);
                        TakeDamageEvent?.Invoke();
                        Destroy(collidedObj);
                    }
                }
            }
        }
        
        private void TakeDamage(int damage)
        {
            if (takeDamage)
            {
                hp -= damage;
                Debug.Log($"{gameObject.name} has taken damage: {damage}, current hp: {hp}");
                if (hp <= 0)
                {
                
                }
            }
        }
    }
}
