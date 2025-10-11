using System;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    [Header("references")]
    [SerializeField] private Animator animator;
    
    
    [Header("Movement settings")]
    [SerializeField] private float speed;
    [SerializeField] private int hp;
    
    //private stuff
    private InputManager inputManager;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    
    private Vector2 moveDirection;

    private void Start()
    {
        inputManager = InputManager.Instance;
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        UpdateSprite();
    }

    private void FixedUpdate()
    { 
        moveDirection = inputManager.MoveAction.ReadValue<Vector2>();
        rb.AddForce(moveDirection * speed, ForceMode2D.Force);
    }

    private void UpdateSprite() //maybe place in separate class to be reused by different sprites
    {
        if (rb.linearVelocity.magnitude >= 1f) animator.Play("WalkAnimation");
        else animator.Play("IdleAnimation");

        if (rb.linearVelocity.x < -1f) spriteRenderer.flipX = true;
        else if (rb.linearVelocity.x > 1f) spriteRenderer.flipX = false;
    }
}
