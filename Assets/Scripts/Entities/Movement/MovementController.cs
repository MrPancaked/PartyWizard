using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;
using Vector2 = UnityEngine.Vector2;

namespace Player
{
    public class MovementController : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D rb;
        private PlayerController playerController;
        [HideInInspector] public ScriptableObjects.Player.MovementData movementData; //public so playercontroller can update the controller data classes
    
        [SerializeField] private Animator animator;
        [SerializeField] private SpriteRenderer spriteRenderer;
    
        [HideInInspector] public Vector2 moveDirection;

        private void Start()
        {
            playerController = GetComponent<PlayerController>();
            movementData = playerController.movementData;
        }
        private void FixedUpdate()
        {
            Move();
            UpdateSprite();
        }
        public void Move()
        {
            rb.linearDamping = movementData.friction;
            rb.AddForce(moveDirection * movementData.speed, ForceMode2D.Force);
        }
    
        private void UpdateSprite() //maybe place in separate class to be reused by different sprites
        {
            if (moveDirection.magnitude > 0f) animator.Play("WalkAnimation");
            else animator.Play("IdleAnimation");

            if (moveDirection.x < -0.1f) spriteRenderer.flipX = true;
            else if (moveDirection.x > 0.1f) spriteRenderer.flipX = false;
        }
    }
}
