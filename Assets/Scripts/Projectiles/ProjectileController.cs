using System;
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
        [HideInInspector] public ScriptableObjects.Player.SpellData spellData;
        private float timeAlive;

        public void Initiate(Vector2 playerPosition, Vector2 mousePosition)
        {
            rb.linearDamping = 0f;
            rb.gravityScale = 0f;
            direction = (mousePosition - playerPosition).normalized;
            speed = spellData.startSpeed;
            timeAlive = 0;
            SetRigidbodyValues();
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
    }
}
