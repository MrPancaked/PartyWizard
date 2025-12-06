using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        public static PlayerController Instance;
        private InputManager inputManager;

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
            if (inputManager != null)
            {
                inputManager.MoveAction.performed += Move;
                inputManager.MoveAction.canceled += Move;
        
                inputManager.AttackAction.performed += Attack;
            }
        }
        private void OnDestroy()
        {
            if (inputManager != null)
            {
                inputManager.MoveAction.performed -= Move;
                inputManager.MoveAction.canceled -= Move;
        
                inputManager.AttackAction.performed += Attack;
            }
        }
        public void Move(InputAction.CallbackContext context)
        {
            if (movementController != null)
            {
                movementController.moveDirection = context.ReadValue<Vector2>();
            }
            else Debug.LogWarning("MovementController is null");
        }
    
    
        public void Attack(InputAction.CallbackContext context)
        {
            if (attackController != null)
            {
                attackController.Attack();
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