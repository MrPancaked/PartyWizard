using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        public Action ActivateShieldEvent;
        public static PlayerController Instance;

        public ScriptableObjects.Player.MovementData movementData;
        public MovementController movementController;
        
        public AttackController attackController;
        public HpController hpController;
        
        InputManager inputManager;

        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return;} // Singleton
            Instance = this;
        }
        
        private void OnDisable()
        {
            inputManager.MoveAction.performed -= Move;
            inputManager.MoveAction.canceled -= Move;
            inputManager.HealAction.performed -= Heal;
            inputManager.ShieldAction.performed -= Shield;
    
            inputManager.AttackAction.performed -= Attack;
            inputManager.AttackAction.canceled -= StopAttack;
        }

        private void Start()
        {
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
            
            attackController = gameObject.GetComponent<AttackController>();
            movementController = gameObject.GetComponent<MovementController>();
            hpController = gameObject.GetComponent<HpController>();
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
            if (Inventory.Instance != null)
            {
                foreach (Item item in Inventory.Instance.Items)
                {
                    if (item.ItemName == $"Magic Shield Bubble")
                    {
                        ActivateShieldEvent?.Invoke();
                        Debug.Log($"Invoking ActivateShieldEvent on {this}");
                        Inventory.Instance.RemoveItem(item);
                        break;
                    }
                }
            } 
        }

        //public void UpdateControllerData()
        //{
        //    movementController.movementData = this.movementData;
        //    attackController.spellData =  this.spellData;
        //}
    }
}