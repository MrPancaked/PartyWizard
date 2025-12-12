using System;
using System.Linq;
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
        public HpController hpController;
        
        public static Action healEvent;

        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return;} // Singleton
            Instance = this;
        }

        private void OnEnable()
        {
            if (InputManager.Instance != null)
            {
                InputManager.Instance.MoveAction.performed += Move;
                InputManager.Instance.MoveAction.canceled += Move;
                InputManager.Instance.HealAction.performed += Heal;
        
                InputManager.Instance.AttackAction.performed += Attack;
            }
            else Debug.LogWarning($"OnEnable {name} did not find InputManager.Instance");
        }
        private void OnDisable()
        {
            if (InputManager.Instance != null)
            {
                InputManager.Instance.MoveAction.performed -= Move;
                InputManager.Instance.MoveAction.canceled -= Move;
                InputManager.Instance.HealAction.performed -= Heal;
        
                InputManager.Instance.AttackAction.performed -= Attack;
            }
            else Debug.LogWarning($"Ondisable: {name} did not find InputManager.Instance");
        }

        private void Start()
        {
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
                attackController.Attack();
            }
            else Debug.LogWarning($"AttackController is null {context}");
        }

        public void Heal(InputAction.CallbackContext context)
        {
            if (hpController != null)
            {
                foreach (Item item in Inventory.Instance.Items)
                {
                    if (item.ItemName == $"Health Potion" && hpController.hp < hpController.hpData.maxHp)
                    {
                        hpController.Heal(5);
                        Inventory.Instance.RemoveItem(item);
                        break;
                    }
                }
            }
            else Debug.LogWarning($"HpController is null {context}");
        }

        //public void UpdateControllerData()
        //{
        //    movementController.movementData = this.movementData;
        //    attackController.spellData =  this.spellData;
        //}
    }
}