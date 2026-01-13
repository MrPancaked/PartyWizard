using System;
using System.Collections;
using Player;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Projectiles
{
    public class ProjectileController : MonoBehaviour 
    {
        //THE WAY THE PROJECTILE SPEED AND DIRECTION GETS MANAGED COULD ALSO BE HANDLED USING THE RB SYSTEM INSTEAD OF MANUALLY PROGRAMING BEHAVIOUR
    
        [SerializeField] private Rigidbody2D rb;
        [HideInInspector] public float speed;
        [HideInInspector] public Vector2 direction;
        public ScriptableObjects.Player.SpellData spellData;
        [SerializeField] private ParticleSystem destroyEffectObject;
        
        private float timeAlive;
        private bool dying;
        private float linearSpeedChange;
        private Vector2 randomCurveDirection;

        [HideInInspector] public bool hurtPlayer;
        [HideInInspector] public bool hurtEnemy;
        
        private void OnCollisionEnter2D(Collision2D other)
        {
            OnParticleDeath();
        }

        private void OnParticleDeath()
        {
            if (!dying)
            {
                dying = true;
                if (spellData.aoeEffect)
                    AoeEffect();
                DoParticles();
                if (!spellData.melee) Destroy(gameObject);
                else StartCoroutine(MeleeDestroyDelay());
            }
        }

        private void AoeEffect() //do AOE stuff when destroyed could be put in separate method to be used multiple times among the spells path
        {
            if (spellData.aoeEffect)
            {
                AudioManager.Instance.PlayOneShot(FMODEvents.Instance.explosionSound, gameObject.transform.position);
                LayerMask layerMask = 0;
                if (hurtEnemy) layerMask |= LayerMask.GetMask("Enemy");
                if (hurtPlayer) layerMask |=  LayerMask.GetMask("Player");
                if (spellData.hurtProjectile) layerMask |= LayerMask.GetMask("Projectile");
                
                Vector2 explosionPos = transform.position;
                Collider2D[] colliders = Physics2D.OverlapCircleAll(explosionPos, spellData.aoeRadius, layerMask);
                foreach (Collider2D hit in colliders)
                {
                    Rigidbody2D hitRb = hit.GetComponent<Rigidbody2D>();
                    if (hitRb != null)
                    {
                        Vector2 hitRbPos = hitRb.transform.position;
                        //hitRb.AddForce((hitRbPos - explosionPos).normalized * spellData.aoePower, ForceMode2D.Impulse);
                    }
                    HpController hpController = hit.gameObject.GetComponent<HpController>();
                    if (hpController != null)
                    {
                        TakeDamageData takeDamageData = new TakeDamageData(spellData.aoeDamage, explosionPos, spellData.aoeKnockback);
                        hpController.TakeDamage(takeDamageData);
                    }
                }
            }
        }
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
                OnParticleDeath();
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
        }

        private void SetRigidbodyValues()
        {
            rb.linearVelocity = direction * speed;
            gameObject.transform.up = direction;
        }

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
