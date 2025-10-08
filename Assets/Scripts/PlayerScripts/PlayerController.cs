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
    
    //private stuff
    private InputManager inputManager;
    private Rigidbody2D rb;
    
    private Vector2 moveDirection;

    private void OnEnable()
    {
        inputManager = InputManager.Instance;
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        moveDirection = inputManager.MoveAction.ReadValue<Vector2>();
        
        rb.AddForce(moveDirection * speed, ForceMode2D.Force);
    }
    
    
}
