using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        public InputManager inputManager {get; private set;}

        public ScriptableObjects.Player.MovementData movementData;
        public MovementController movementController;
    
        public ScriptableObjects.Player.SpellData spellData;
        public AttackController attackController;
    
        public ScriptableObjects.Player.HpData hpData;
        public HpController hpController;

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

        public void SetHp()
        {
            if (hpController != null)
            {
                hpController.Initialize();
            }
        }

        public void UpdateControllerData()
        {
            movementController.movementData = this.movementData;
            attackController.spellData =  this.spellData;
            hpController.hpData = this.hpData;
        }
    }
}