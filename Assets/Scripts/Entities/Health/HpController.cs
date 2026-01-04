
using UnityEngine;
using System;
using FMOD.Studio;
using Projectiles;
using ScriptableObjects.Player;

namespace Player
{
    public class HpController : MonoBehaviour
    {
        public static Action InitiateBossHealthBar;
        public event Action<TakeDamageData> TakeDamageEvent;
        public event Action<TakeDamageData> HealEvent;
        public event Action UpdatedMaxHealth;
        public event Action DeathEvent;
        
        public int maxHp {get; private set;}
        public int hp {get; private set;}
        public int shield {get; private set;}
        public int contactDamage {get; private set;}
        public float contactKnockback {get; private set;}
        public bool takeDamage {get; private set;}
        public HpData hpData;
        private bool dead = false;
        public bool isPlayer { get; private set; }
        public bool isEnemy { get; private set; }
        public bool isBoss { get; private set; }
        
        public GameObject magicShieldBubble;
        private bool shieldActive = false;

        private void Awake()
        {
            Initialize();
        }

        private void Start()
        {
            if (isPlayer)
            {
                EventBus<ExtraHpUpgradeEventData>.OnNoParamEventPublished += UpdateHp;
                if (magicShieldBubble != null)
                {
                    PlayerController.Instance.ActivateShieldEvent += ActivateMagicShield;
                    Debug.Log($"Subscribed HpController on {PlayerController.Instance}");
                }
                else Debug.LogWarning($"{magicShieldBubble} is null");
            }
            else if (isBoss)
            {
                InitiateBossHealthBar?.Invoke();
            }
        }

        private void OnDisable()
        {
            if (isPlayer)
            {
                EventBus<ExtraHpUpgradeEventData>.OnNoParamEventPublished -= UpdateHp;
                if (magicShieldBubble != null) PlayerController.Instance.ActivateShieldEvent -= ActivateMagicShield;
            }
        }

        public void Initialize()
        {
            if (shieldActive) BreakMagicShield();
            isPlayer = gameObject.CompareTag("Player");
            isBoss = gameObject.CompareTag("Boss");
            isEnemy = gameObject.layer == LayerMask.NameToLayer("Enemy");
            maxHp = hpData.maxHp;
            if (hpData.startHp > maxHp) hpData.startHp = maxHp;
            hp = hpData.startHp;
            shield = hpData.startShield;
            contactDamage = hpData.contactDamage;
            contactKnockback = hpData.contactKnockback;
            takeDamage = hpData.takeDamage;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            GameObject collidedObj = collision.gameObject;
            if (isPlayer)
            {
                if (collidedObj.layer == LayerMask.NameToLayer("Enemy"))
                {
                    HpController otherHpController = collidedObj.GetComponent<HpController>();
                    int receivedDamage = otherHpController.contactDamage;
                    
                    TakeDamageData takeDamageData = new TakeDamageData(receivedDamage, collidedObj.transform.position, otherHpController.contactKnockback);
                    TakeDamage(takeDamageData);
                    
                }
                else if (collidedObj.layer == LayerMask.NameToLayer("Projectile"))
                {
                    ProjectileController projectileController = collidedObj.GetComponent<ProjectileController>();
                    SpellData spellData = projectileController.spellData;
                    if (projectileController.hurtPlayer)
                    {
                        int receivedDamage = spellData.damage;
                        TakeDamageData takeDamageData = new TakeDamageData(receivedDamage, collidedObj.transform.position, spellData.knockback);
                        TakeDamage(takeDamageData);
                    }
                }
            }
            else if (isEnemy)
            {
                if (collidedObj.layer == LayerMask.NameToLayer("Player"))
                {
                    HpController otherHpController = collidedObj.GetComponent<HpController>();
                    int receivedDamage = otherHpController.contactDamage;
                    
                    TakeDamageData takeDamageData = new TakeDamageData(receivedDamage, collidedObj.transform.position, otherHpController.contactKnockback);
                    TakeDamage(takeDamageData);
                }
                else if (collidedObj.layer == LayerMask.NameToLayer("Projectile"))
                {
                    ProjectileController projectileController = collidedObj.GetComponent<ProjectileController>();
                    SpellData spellData = projectileController.spellData;
                    if (projectileController.hurtEnemy)
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
            if (!isPlayer)
            {
                PlayerStatController playerStatController = PlayerStatController.Instance;
                damage += playerStatController.flatDamage;
                damage *= playerStatController.damageMultiplier;
            }
            if (takeDamage)
            {
                if (!shieldActive)
                {
                    shield -= damage;
                    if (shield < 0)
                    {
                        hp -= -shield;
                        shield = 0;
                        if (hp < 0) hp = 0;
                    }
                    Debug.Log($"{gameObject.name} has taken damage: {damage}, current hp: {hp}");
                    
                    if (isPlayer && damage > 0) AudioManager.Instance.PlayOneShot(FMODEvents.Instance.playerHurtSound, gameObject.transform.position);
                    else if (damage > 0) AudioManager.Instance.PlayOneShot(FMODEvents.Instance.enemyHurtSound, gameObject.transform.position);
                    
                    TakeDamageEvent?.Invoke(takeDamageData);
                    if (hp == 0)
                    {
                        if (!dead) Die();
                        dead = true;
                    }
                }
                else
                {
                    BreakMagicShield();
                    Debug.Log("broke Magic Shield");
                }
            }
        }

        public void Heal(int healAmount)
        {
            if (hp + healAmount > maxHp)
            {
                int maxHeal = maxHp - hp;
                healAmount = maxHeal;
            }
            hp += healAmount;
            AudioManager.Instance.PlayOneShot(FMODEvents.Instance.healSound, gameObject.transform.position);
            Debug.Log($"{gameObject.name} has healed {healAmount}");
            HealEvent?.Invoke(new TakeDamageData(-healAmount));
        }

        private void ActivateMagicShield()
        {
            Debug.Log("Activating magic shield");
            magicShieldBubble.SetActive(true);
            shieldActive = true;
            AudioManager.Instance.PlayOneShot(FMODEvents.Instance.shieldCastSound, gameObject.transform.position);
        }

        private void BreakMagicShield()
        {
            magicShieldBubble.SetActive(false);
            shieldActive = false;
            AudioManager.Instance.PlayOneShot(FMODEvents.Instance.shieldBreakSound, gameObject.transform.position);
        }
        private void Die()
        {
            DeathEvent?.Invoke();
            if (isPlayer)
            {
                EventBus<PlayerDieEventData>.Publish(new PlayerDieEventData(gameObject));
                AudioManager.Instance.PlayOneShot(FMODEvents.Instance.deathSound, gameObject.transform.position);
            }
            else if (isEnemy) 
            {
                EventBus<EnemyDieEventData>.Publish(new EnemyDieEventData(gameObject));
                
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

        private void UpdateHp()
        {
            maxHp += PlayerStatController.Instance.extraHpUpgrade;
            UpdatedMaxHealth?.Invoke();
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
