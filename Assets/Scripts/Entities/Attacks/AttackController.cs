using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class AttackController : MonoBehaviour
    {
        private PlayerController playerController;
        
        [Header("Spell amount Settings")]
        public int amount;
        public float angle;
        public float delayBetweenSpells;
    
        [SerializeField] private GameObject projectile;
        [SerializeField] private Transform projectileParent;
    
        private void Start()
        {
            playerController = GetComponent<PlayerController>();
            if (amount < 1) amount = 1;
        }
        public void Attack()
        {
            Vector2 playerPosition = playerController.transform.position;
            Vector2 mousePosition = Vector2.zero;
            if (Camera.main != null) mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            else Debug.LogWarning("Main Camera not found!");
            Vector2 direction = (mousePosition - playerPosition).normalized;
            if (amount > 1)
            {
                if (angle != 0) direction = RotateVector(direction, -angle / 2f);
                float angleInterval = angle / (amount - 1); //division by 0 is not a problem because amount is never 1 due to the check earlier
                for (int i = 0; i < amount; i++)
                {
                    Vector2 castDirection = RotateVector(direction, angleInterval * i);
                    var projectileInstance = Instantiate(projectile, transform.position, transform.rotation, projectileParent);
                    var projectileController = projectileInstance.GetComponent<Projectiles.ProjectileController>();
                    projectileController.Initiate(castDirection);
                }
            }
            else if (amount == 1)
            {
                var projectileInstance = Instantiate(projectile, transform.position, transform.rotation, projectileParent);
                var projectileController = projectileInstance.GetComponent<Projectiles.ProjectileController>();
                projectileController.Initiate(direction);
            }
            else Debug.LogError("somehow trying to cast invalid amount of spells");
        }

        private Vector2 RotateVector(Vector2 direction, float angle)
        {
            angle = 2 * Mathf.PI * angle / 360f; //convert to radians
            float newXDir = direction.x * Mathf.Cos(angle) - direction.y * Mathf.Sin(angle);
            float newYDir = direction.x * Mathf.Sin(angle) + direction.y * Mathf.Cos(angle);
            return new  Vector2(newXDir, newYDir);
        }
    }
}
