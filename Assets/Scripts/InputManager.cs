using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    
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
    }
    private void OnDisable()
    {
        InputSystem.actions.Disable();
    }
}
