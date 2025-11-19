using System;
using Player;
using UnityEngine;

namespace Enemies
{
    public class FloatingMoveBehaviour : EnemyMoveBehaviour
    {
        [SerializeField] private PlayerController playerController;
        protected override void Start()
        {
            base.Start();
            if (playerController == null) playerController = PlayerController.Instance;
        }
        private void FixedUpdate()
        {
            Move();
        }
        protected override void Move()
        {
            base.Move();
            direction = (playerController.transform.position - this.transform.position).normalized;
            rb.AddForce(direction * speed, ForceMode2D.Force);
        }
    }
}