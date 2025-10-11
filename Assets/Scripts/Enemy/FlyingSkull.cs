using System;
using UnityEngine;

public class FlyingSkull : MonoBehaviour
{
    private Rigidbody2D rb;
    private PlayerController playerController;
    private SpriteRenderer spriteRenderer;

    [Header("settings")] 
    [SerializeField] private EntitySettings skullSettings;
    [SerializeField] private float acceleration;
    
    //variables
    private Vector3 direction;
    private HpManager hpManager;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerController = PlayerController.Instance;
        spriteRenderer = GetComponent<SpriteRenderer>();
        hpManager = new HpManager(skullSettings);
    }
    private void FixedUpdate()
    {
        ChasePlayer();
        UpdateSprite();
    }
    private void ChasePlayer()
    {
        direction = (playerController.transform.position - this.transform.position).normalized;
        rb.AddForce(direction * acceleration, ForceMode2D.Force);
    }
    
    private void UpdateSprite() //maybe place in separate class to be reused by different sprites
    {
        if (direction.x < -0.1f) spriteRenderer.flipX = true;
        else if (direction.x > 0.1f) spriteRenderer.flipX = false;
    }
}
