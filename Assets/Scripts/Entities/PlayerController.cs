using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    /// <summary>
    /// Class to manage translating player inputs from the InputManager to player actions
    /// manages inputs for:
    /// - movement
    /// - attacks
    /// - healing
    /// - magic shield
    /// </summary>
    public class PlayerController : MonoBehaviour
    {
        public Action ActivateShieldEvent;
        public static PlayerController Instance;

        public ScriptableObjects.Player.MovementData movementData;
        public MovementController movementController;
        
        public AttackController attackController;
        public HpController hpController;
        
        private InputManager inputManager;

        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return;} // Singleton
            Instance = this;
        }
        
        private void OnDisable()
        {
            // unsubscribing inputs
            if (inputManager != null)
            {
                inputManager.MoveAction.performed -= Move;
                inputManager.MoveAction.canceled -= Move;
                inputManager.HealAction.performed -= Heal;
                inputManager.ShieldAction.performed -= Shield;
    
                inputManager.AttackAction.performed -= Attack;
                inputManager.AttackAction.canceled -= StopAttack;
            }
        }

        private void Start()
        {
            //subscribe to inputs
            if (InputManager.Instance != null)
            {
                inputManager = InputManager.Instance;
                inputManager.MoveAction.performed += Move;
                inputManager.MoveAction.canceled += Move;
                inputManager.HealAction.performed += Heal;
                inputManager.ShieldAction.performed += Shield;
        
                inputManager.AttackAction.performed += Attack;
                inputManager.AttackAction.canceled += StopAttack;
            }
            else Debug.LogWarning($"OnEnable {name} did not find InputManager.Instance");
            
            // settings components
            attackController = gameObject.GetComponent<AttackController>();
            movementController = gameObject.GetComponent<MovementController>();
            hpController = gameObject.GetComponent<HpController>();
        }

        // set movements controllers moveDirection in the direction of the inputs
        public void Move(InputAction.CallbackContext context)
        {
            if (movementController != null)
            {
                movementController.moveDirection = context.ReadValue<Vector2>();
            }
            else Debug.LogWarning("MovementController is null");
        }
        
        // methods to start and stop attacking by setting wantsToAttack in the attackcontroller
        public void Attack(InputAction.CallbackContext context)
        {
            if (attackController != null)
            {
                attackController.wantsToAttack = true;
            }
            else Debug.LogWarning($"AttackController is null {context}");
        }
        public void StopAttack(InputAction.CallbackContext context)
        {
            if (attackController != null)
            {
                attackController.wantsToAttack = false;
            }
            else Debug.LogWarning($"AttackController is null {context}");
        }

        
        public void Heal(InputAction.CallbackContext context)
        {
            if (hpController != null && Inventory.Instance != null)
            {
                foreach (Item item in Inventory.Instance.Items)
                {
                    if (item.ItemName == $"Health Potion")
                    {
                        hpController.Heal(5);
                        Inventory.Instance.RemoveItem(item);
                        break;
                    }
                }
            }
            else Debug.LogWarning($"HpController or Inventory is null {context}");
        }

        public void Shield(InputAction.CallbackContext context)
        {
            if (Inventory.Instance != null && Inventory.Instance != null)
            {
                foreach (Item item in Inventory.Instance.Items)
                {
                    if (item.ItemName == $"Magic Shield Bubble")
                    {
                        ActivateShieldEvent?.Invoke();
                        Inventory.Instance.RemoveItem(item);
                        break;
                    }
                }
            } 
        }
    }
}