
using UnityEngine;
using System;
using Projectiles;
using ScriptableObjects.Player;

namespace Player
{
    public class HpController : MonoBehaviour
    {
        public event Action<TakeDamageData> TakeDamageEvent;
        public event Action<int> HealEvent;
        public event Action DeathEvent;
        
        public int hp {get; private set;}
        public int shield {get; private set;}
        public int contactDamage {get; private set;}
        public float contactKnockback {get; private set;}
        public bool takeDamage {get; private set;}
        public ScriptableObjects.Player.HpData hpData; //public so playercontroller can update the controller data classes
        private bool dead = false;
    
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
            contactKnockback = hpData.contactKnockback;
            takeDamage = hpData.takeDamage;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            GameObject collidedObj = collision.gameObject;
            if (gameObject.CompareTag("Player"))
            {
                if (collidedObj.CompareTag("Enemy"))
                {
                    HpController otherHpController = collidedObj.GetComponent<HpController>();
                    int receivedDamage = otherHpController.contactDamage;
                    
                    TakeDamageData takeDamageData = new TakeDamageData(receivedDamage, collidedObj.transform.position, otherHpController.contactKnockback);
                    TakeDamage(takeDamageData);
                    
                }
                else if (collidedObj.CompareTag("Projectile"))
                {
                    SpellData spellData = collidedObj.GetComponent<ProjectileController>().spellData;
                    if (spellData.hurtPlayer)
                    {
                        int receivedDamage = spellData.damage;
                        TakeDamageData takeDamageData = new TakeDamageData(receivedDamage, collidedObj.transform.position, spellData.knockback);
                        TakeDamage(takeDamageData);
                    }
                }
            }
            else if (gameObject.CompareTag("Enemy"))
            {
                if (collidedObj.CompareTag("Player"))
                {
                    HpController otherHpController = collidedObj.GetComponent<HpController>();
                    int receivedDamage = otherHpController.contactDamage;
                    
                    TakeDamageData takeDamageData = new TakeDamageData(receivedDamage, collidedObj.transform.position, otherHpController.contactKnockback);
                    TakeDamage(takeDamageData);
                }
                else if (collidedObj.CompareTag("Projectile"))
                {
                    SpellData spellData = collidedObj.GetComponent<ProjectileController>().spellData;
                    if (spellData.hurtEnemy)
                    {
                        int receivedDamage = spellData.damage;
                        TakeDamageData takeDamageData = new TakeDamageData(receivedDamage, collidedObj.transform.position, spellData.knockback);
                        TakeDamage(takeDamageData);
                    }
                }
            }
        }
        
        public void TakeDamage(TakeDamageData takeDamageData)
        {
            int damage = takeDamageData.damage;
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
                
                TakeDamageEvent?.Invoke(takeDamageData);
                
                if (hp == 0)
                {
                    if (!dead) Die();
                    dead = true;
                }
            }
        }

        public void Heal(int healAmount)
        {
            if (hp + healAmount > hpData.maxHp)
            {
                int maxHeal = hpData.maxHp - hp;
                healAmount = maxHeal;
            }
            hp += healAmount;
            Debug.Log($"{gameObject.name} has healed {healAmount}");
            HealEvent?.Invoke(healAmount);
        }
        private void Die()
        {
            DeathEvent?.Invoke();
            if (gameObject.CompareTag("Player")) EventBus<PlayerDieEventData>.Publish(new PlayerDieEventData(gameObject)); //deathEvent.Publish(new PlayerDieEventData(gameObject), this.gameObject);
            else if (gameObject.CompareTag("Enemy")) 
            {
                EventBus<EnemyDieEventData>.Publish(new EnemyDieEventData(gameObject)); //deathEvent.Publish(new EnemyDieEventData(gameObject), this.gameObject);
                
                if (hpData.destroyEffectObject != null) 
                    Instantiate(hpData.destroyEffectObject, transform.position, transform.rotation);
                
                Destroy(gameObject);
            }
            else
            {
                if (hpData.destroyEffectObject != null) 
                    Instantiate(hpData.destroyEffectObject, transform.position, transform.rotation);
                
                Destroy(gameObject);
            }
        }
    }

    public class TakeDamageData
    {
        public int damage { get; private set; }
        public Vector2 hitLocation { get; private set; }
        public float power { get; private set; }
        public TakeDamageData(int damage, Vector2 hitLocation, float power)
        {
            this.damage = damage;
            this.hitLocation = hitLocation;
            this.power = power;
        }
        public TakeDamageData(int damage)
        {
            this.damage = damage;
        }
    }
}
