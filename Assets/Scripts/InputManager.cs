using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    
    [SerializeField] private Player.PlayerController playerController;
    
    //Actions
    private InputAction moveAction;
    public InputAction MoveAction => moveAction;

    private InputAction attackAction;
    public InputAction AttackAction => attackAction;

    
    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; } // Singleton
        Instance = this;
        
        moveAction = InputSystem.actions.FindAction("Move");
        attackAction = InputSystem.actions.FindAction("Attack");
    }
    private void OnEnable()
    {
        InputSystem.actions.Enable();
        
        moveAction.performed += playerController.Move;
        moveAction.canceled += playerController.Move;
        
        attackAction.performed += playerController.Attack;
    }
    private void OnDisable()
    {
        InputSystem.actions.Disable();
        
        moveAction.performed -= playerController.Move;
        attackAction.performed -= playerController.Attack;
        
        attackAction.performed -= playerController.Attack;
        
    }
}
