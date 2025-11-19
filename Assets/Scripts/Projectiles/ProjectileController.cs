using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

namespace Projectiles
{
    public class ProjectileController : MonoBehaviour 
    {
        //THE WAY THE PROJECTILE SPEED AND DIRECTION GETS MANAGED COULD ALSO BE HANDLED USING THE RB SYSTEM INSTEAD OF MANUALLY PROGRAMING BEHAVIOUR
    
        [SerializeField] private Rigidbody2D rb;
        [HideInInspector] public float speed;
        [HideInInspector] public Vector2 direction;
        public ScriptableObjects.Player.SpellData spellData;
        private float timeAlive;
        
        private void OnDestroy()
        {
            LayerMask layerMask = 0;
            if (spellData.hurtEnemy && !spellData.hurtPlayer) layerMask = LayerMask.GetMask("Enemy");
            else if (spellData.hurtPlayer && !spellData.hurtEnemy) layerMask = LayerMask.GetMask("Player");
            else if (spellData.hurtPlayer && spellData.hurtEnemy) layerMask = LayerMask.GetMask("Player") | LayerMask.GetMask("Enemy");
            
            Vector2 explosionPos = transform.position;
            Collider2D[] colliders = Physics2D.OverlapCircleAll(explosionPos, spellData.aoeRadius, layerMask);
            foreach (Collider2D hit in colliders)
            {
                Rigidbody2D hitRb = hit.GetComponent<Rigidbody2D>();
                if (hitRb != null)
                {
                    Vector2 hitRbPos = hitRb.transform.position;
                    hitRb.AddForce((hitRbPos - explosionPos).normalized * spellData.aoePower, ForceMode2D.Impulse);
                }
            }
        }
        public void Initiate(Vector2 playerPosition, Vector2 mousePosition)
        {
            rb.linearDamping = 0f;
            rb.gravityScale = 0f;
            direction = (mousePosition - playerPosition).normalized;
            speed = spellData.startSpeed;
            timeAlive = 0;
            SetRigidbodyValues();
            InitiateCollider();
        }
        private void FixedUpdate()
        {
            SetDirection();
            SetSpeed();

            SetRigidbodyValues();
        
            timeAlive += Time.fixedDeltaTime;
            if (timeAlive >= spellData.maxTimeAlive) Destroy(gameObject);
        }

        private void SetSpeed()
        {
            switch (spellData.speedScaling)
            {
                case (ScriptableObjects.Player.SpellData.SpeedScaling.None):
                {
                    break;
                }
                case (ScriptableObjects.Player.SpellData.SpeedScaling.LinearInc):
                {
                    //TODO: add scaling option
                    break;
                }
                case (ScriptableObjects.Player.SpellData.SpeedScaling.LinearDec):
                {
                    //TODO: add scaling option
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
                //TODO: add scaling options
            }
        }

        private void SetRigidbodyValues()
        {
            rb.linearVelocity = direction * speed;
        }

        private void InitiateCollider()
        {
            Collider2D collider = gameObject.GetComponent<Collider2D>();
            if (collider != null)
            {
                Debug.Log($"has collider : {collider.name}");
                int mask = 0;
                if (!spellData.hurtEnemy) mask |= LayerMask.GetMask("Enemy");
                if (!spellData.hurtPlayer) mask |= LayerMask.GetMask("Player");
                collider.excludeLayers = mask;
            }
        }
    }
}
