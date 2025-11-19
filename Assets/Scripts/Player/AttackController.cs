using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class AttackController : MonoBehaviour
    {
        private PlayerController playerController;
    
        [SerializeField] private GameObject projectile;
    
        private void Start()
        {
            playerController = GetComponent<PlayerController>();
        }
        public void Attack()
        {
            var projectileInstance = Instantiate(projectile, transform.position, transform.rotation);
            Vector2 playerPosition = playerController.transform.position;
            Vector2 mousePosition = Vector2.zero; 
            if (Camera.main != null) mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            else Debug.LogWarning("Main Camera not found!");
            //change to work with inputmanager? but idk maybe this is more efficient and basically the same?
        
            var projectileController = projectileInstance.GetComponent<Projectiles.ProjectileController>();
            projectileController.Initiate(playerPosition, mousePosition);
        }
    }
}
