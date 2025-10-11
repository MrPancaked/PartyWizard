using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    
    //Actions
    private InputAction moveAction;
    public InputAction MoveAction => moveAction;

    
    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; } // Singleton
        Instance = this;
        
        moveAction = InputSystem.actions.FindAction("Movement");
    }
    private void OnEnable()
    {
        var actionArray = InputSystem.actions;
        foreach (var action in actionArray)
        {
            action.Enable();
        }
        moveAction.Enable();
    }
    private void OnDisable()
    {
        var actionArray = InputSystem.actions;
        foreach (var action in actionArray)
        {
            action.Disable();
        }
        moveAction.Disable();
    }
}
