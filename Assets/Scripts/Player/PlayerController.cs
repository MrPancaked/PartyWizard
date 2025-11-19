using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        public static PlayerController Instance;
        public InputManager inputManager {get; private set;}

        public ScriptableObjects.Player.MovementData movementData;
        public MovementController movementController;
        
        public AttackController attackController;

        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return;} // Singleton
            Instance = this;
        }
        private void Start()
        {
            inputManager = InputManager.Instance;
        }
    
        public void Move(InputAction.CallbackContext context)
        {
            if (movementController != null)
            {
                movementController.moveDirection = context.ReadValue<Vector2>();
                Debug.Log("trying to walk");
            }
            else Debug.LogWarning("MovementController is null");
        }
    
    
        public void Attack(InputAction.CallbackContext context)
        {
            if (attackController != null)
            {
                attackController.Attack();
                Debug.Log("trying to cast spell");
            }
            else Debug.LogWarning("AttackController is null");
        }

        //public void UpdateControllerData()
        //{
        //    movementController.movementData = this.movementData;
        //    attackController.spellData =  this.spellData;
        //}
    }
}