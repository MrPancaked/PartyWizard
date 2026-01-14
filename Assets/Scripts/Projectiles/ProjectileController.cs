using System;
using Player;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Projectiles
{
    /*
     * ProjectileController takes care of projectile behavior.
     * It takes core projectile information from the SpellData scriptableObject
     */
    public class ProjectileController : MonoBehaviour 
    {
        [SerializeField] private Rigidbody2D rb;
        [HideInInspector] public float speed;
        [HideInInspector] public Vector2 direction;
        public ScriptableObjects.Player.SpellData spellData;
        [SerializeField] private ParticleSystem destroyEffectObject;
        
        private float timeAlive;
        private float linearSpeedChange;
        private Vector2 randomCurveDirection;

        [HideInInspector] public bool hurtPlayer;
        [HideInInspector] public bool hurtEnemy;
        
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (spellData.aoeEffect)
                AoeEffect();
            DoParticles();
            Destroy(gameObject);
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
            hurtPlayer = isEnemy;
            hurtEnemy = isPlayer;
            transform.position = new Vector3(transform.position.x, transform.position.y, -0.1f);
            rb.linearDamping = 0f;
            rb.gravityScale = 0f;
            speed = spellData.startSpeed;
            direction = castDirection;
            timeAlive = 0;
            
            linearSpeedChange = (spellData.endSpeed - spellData.startSpeed) / spellData.maxTimeAlive;
            randomCurveDirection = Random.insideUnitCircle.normalized;
            
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
                AoeEffect();
                DoParticles();
                Destroy(gameObject);
            }
        }

        private void SetSpeed()
        {
            switch (spellData.speedScaling)
            {
                case (ScriptableObjects.Player.SpellData.SpeedScaling.None):
                {
                    break;
                }
                case (ScriptableObjects.Player.SpellData.SpeedScaling.Linear):
                {
                    speed += linearSpeedChange * Time.fixedDeltaTime;
                    break;
                }
            }
        }

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
            gameObject.transform.up = direction;
        }

        private void SetRigidbodyValues()
        {
            rb.linearVelocity = direction * speed;
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
    }
}
