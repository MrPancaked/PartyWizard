using System;
using System.Collections;
using Player;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Projectiles
{
    /// <summary>
    ///ProjectileController takes care of projectile behavior.
    ///It takes core projectile information from the SpellData scriptableObject
    /// </summary>
    public class ProjectileController : MonoBehaviour 
    {
        [SerializeField] private Rigidbody2D rb;
        [HideInInspector] public float speed;
        [HideInInspector] public Vector2 direction;
        private SpeedScaling speedScaling;
        public ScriptableObjects.Player.SpellData spellData;
        [SerializeField] private ParticleSystem destroyEffectObject;
        
        [HideInInspector] public float timeAlive;
        private bool dying;
        private Vector2 randomCurveDirection;

        [HideInInspector] public bool hurtPlayer;
        [HideInInspector] public bool hurtEnemy;
        
        private void OnCollisionEnter2D(Collision2D other)
        {
            OnProjectileDeath();
        }

        private void OnProjectileDeath()
        {
            if (!dying)
            {
                Debug.Log("projectile speed on death: " + speed);
                dying = true;
                if (spellData.aoeEffect)
                    AoeEffect();
                DoParticles();
                if (!spellData.melee) Destroy(gameObject);
                else StartCoroutine(MeleeDestroyDelay());
            }
        }

        //method for Damaging enemies within the range of spellData.aoeRadius
        private void AoeEffect()
        {
            if (spellData.aoeEffect)
            {
                AudioManager.Instance.PlayOneShot(FMODEvents.Instance.explosionSound, gameObject.transform.position);
                
                //Setting up layermask so it makes sure to only hit the right enemies
                LayerMask layerMask = 0;
                if (hurtEnemy) layerMask |= LayerMask.GetMask("Enemy");
                if (hurtPlayer) layerMask |=  LayerMask.GetMask("Player");
                if (spellData.hurtProjectile) layerMask |= LayerMask.GetMask("Projectile");
                
                Vector2 explosionPos = transform.position;
                Collider2D[] colliders = Physics2D.OverlapCircleAll(explosionPos, spellData.aoeRadius, layerMask);
                foreach (Collider2D hit in colliders)
                {
                    HpController hpController = hit.gameObject.GetComponent<HpController>();
                    if (hpController != null)
                    {
                        TakeDamageData takeDamageData = new TakeDamageData(spellData.aoeDamage, explosionPos, spellData.aoeKnockback);
                        hpController.TakeDamage(takeDamageData);
                    }
                }
            }
        }
        //Initializes the projectile
        public void Initiate(Vector2 castDirection, bool isPlayer, bool isEnemy)
        {
            speedScaling = GetComponent<SpeedScaling>();
            
            hurtPlayer = isEnemy;
            hurtEnemy = isPlayer;
            transform.position = new Vector3(transform.position.x, transform.position.y, -0.1f);
            rb.linearDamping = 0f;
            rb.gravityScale = 0f;
            speed = spellData.startSpeed;
            direction = castDirection;
            timeAlive = 0;
            
            randomCurveDirection = Random.insideUnitCircle.normalized;
            
            if (spellData.melee) AudioManager.Instance.PlayOneShot(FMODEvents.Instance.swordSlash, transform.position);
            else AudioManager.Instance.PlayOneShot(FMODEvents.Instance.castSound, transform.position);
            
            SetRigidbodyValues();
            InitiateCollider();
        }
        private void FixedUpdate()
        {
            SetDirection();
            SetSpeed();

            SetRigidbodyValues();
        
            timeAlive += Time.fixedDeltaTime;
            if (timeAlive >= spellData.maxTimeAlive)
            {
                OnProjectileDeath();
            }
        }

        //Sets the projectiles speed based on the SpeedScaling strategy
        private void SetSpeed()
        {
            if (speedScaling != null) // dont do speed scaling if there isnt any
            {
                speed = speedScaling.SetSpeed();
            }
            
        }

        //Sets the projectile's direction based on DirectionChange setting
        private void SetDirection()
        {
            switch (spellData.directionChange)
            {
                case (ScriptableObjects.Player.SpellData.DirectionChange.None):
                {
                    break;
                }
                case (ScriptableObjects.Player.SpellData.DirectionChange.RandomCurve):
                {
                    direction = (direction + spellData.directionChangeStrength * Time.fixedDeltaTime * randomCurveDirection).normalized;
                    break;
                }
                case (ScriptableObjects.Player.SpellData.DirectionChange.RandomWiggle):
                {
                    break;
                }
            }
        }
        
        private void SetRigidbodyValues()
        {
            rb.linearVelocity = direction * speed;
            gameObject.transform.up = direction;
        }

        //Sets right layerExclusion for the projectile
        private void InitiateCollider()
        {
            Collider2D collider = gameObject.GetComponent<Collider2D>();
            if (collider != null)
            {
                int mask = 0;
                if (!hurtEnemy) mask |= LayerMask.GetMask("Enemy");
                if (!hurtPlayer) mask |= LayerMask.GetMask("Player");
                if (!spellData.hurtProjectile) mask |= LayerMask.GetMask("Projectile");
                collider.excludeLayers = mask;
            }
        }

        //method to detach and stop attached particle system
        //the particle system should destroy itself after finishing
        private void DoParticles()
        {
            if (destroyEffectObject != null) 
                Instantiate(destroyEffectObject, transform.position, transform.rotation);
            ParticleSystem particleSystem = gameObject.GetComponentInChildren<ParticleSystem>();
            if (particleSystem != null)
            {
                particleSystem.Stop();
                particleSystem.transform.parent = null;
            }
        }

        //Makes sure the melee attack doesn't destroy on a collision right away
        //also sets collider.enabled to false to make sure the player won't get hit multiple times
        private IEnumerator MeleeDestroyDelay()
        {
            Collider2D collider = gameObject.GetComponent<Collider2D>();
            collider.enabled = false; // dissable melee collider to avoid double hits
            if (timeAlive < spellData.maxTimeAlive)
            {
                yield return new WaitForSeconds(spellData.maxTimeAlive - timeAlive);
                Destroy(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
