using System;
using ScriptableObjects.Player;
using UnityEngine;

namespace Enemies
{
    /// <summary>
    /// Class that was initially meant as enemy movement behaviors
    /// switched to FSM so this only still used by the skull1 enemy which should also go on to use an FSM
    /// </summary>
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

        protected abstract void Move();
    }
}