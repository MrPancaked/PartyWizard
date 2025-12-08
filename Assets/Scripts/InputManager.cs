using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    
    [SerializeField] InputActionAsset inputActionAsset;
    //Actions
    private InputAction moveAction;
    public InputAction MoveAction => moveAction;

    private InputAction attackAction;
    public InputAction AttackAction => attackAction;
    private InputAction pauseGameAction;
    public InputAction PauseGameAction => pauseGameAction;

    
    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; } // Singleton
        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        moveAction = inputActionAsset.FindAction("Move");
        attackAction = inputActionAsset.FindAction("Attack");
        pauseGameAction = inputActionAsset.FindAction("PauseGame");
        Debug.Log($"{name} did awake stuff");
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        inputActionAsset.Enable();
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        inputActionAsset.Enable();
        Debug.Log($"Enabled all input actions on scene loaded");
    }
    private void OnDisable()
    {
        inputActionAsset.Disable();
        Debug.Log($"Disabled all input actions OnDisable");
    }
}
