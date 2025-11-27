using System;
using ScriptableObjects.Player;
using UnityEngine;

namespace Enemies
{
    [RequireComponent(typeof(Rigidbody2D))]
    public abstract class EnemyMoveBehaviour : MonoBehaviour
    {
        [SerializeField] private MovementData movementData;
        protected float speed;
        protected Vector2 direction;
        protected float friction;
        protected Rigidbody2D rb;

        protected virtual void Start()
        {
            speed = movementData.speed;
            friction = movementData.friction;
            rb = GetComponent<Rigidbody2D>();
            rb.linearDamping = friction;
        }

        protected virtual void Move(){}
    }
}