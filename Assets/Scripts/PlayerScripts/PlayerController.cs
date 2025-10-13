using System;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;
    
    //events
    public event Action<>
    
    [Header("references")]
    [SerializeField] private Animator animator;
    [SerializeField] private EntitySettings playerSettings;
    
    private InputManager _inputManager;
    private Rigidbody2D _rb;
    private SpriteRenderer _spriteRenderer;
    private HpManager _hpManager;
    
    [Header("Movement settings")]
    [SerializeField] private float speed;
    
    //variables
    private Vector2 _moveDirection;

    private void OnEnable()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; } // Singleton
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        _inputManager = InputManager.Instance;
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _hpManager = new HpManager(playerSettings);
    }
    private void FixedUpdate()
    { 
        PlayerForces();
        UpdateSprite();
    }
    private void PlayerForces()
    {
        _moveDirection = _inputManager.MoveAction.ReadValue<Vector2>();
        _rb.AddForce(_moveDirection * playerSettings.speed, ForceMode2D.Force);
    }
    private void UpdateSprite() //maybe place in separate class to be reused by different sprites
    {
        if (_rb.linearVelocity.magnitude >= 1f) animator.Play("WalkAnimation");
        else animator.Play("IdleAnimation");

        if (_moveDirection.x < -0.1f) _spriteRenderer.flipX = true;
        else if (_moveDirection.x > 0.1f) _spriteRenderer.flipX = false;
    }
}
