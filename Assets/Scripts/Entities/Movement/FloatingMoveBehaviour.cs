using System;
using Player;
using UnityEngine;

namespace Enemies
{
    public class FloatingMoveBehaviour : EnemyMoveBehaviour
    {
        [SerializeField] private PlayerController playerController;
        private SpriteRenderer spriteRenderer;
        protected override void Start()
        {
            base.Start();
            if (playerController == null) playerController = PlayerController.Instance;
            spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        }
        private void FixedUpdate()
        {
            Move();
            UpdateSprite();
        }
        protected override void Move()
        {
            if (playerController != null) direction = (playerController.transform.position - this.transform.position).normalized;
            else direction = Vector2.zero;
            rb.AddForce(direction * speed, ForceMode2D.Force);
        }
        private void UpdateSprite() //maybe place in separate class to be reused by different sprites
        {
            if (direction.x < -0.1f) spriteRenderer.flipX = true;
            else if (direction.x > 0.1f) spriteRenderer.flipX = false;
        }
    }
}