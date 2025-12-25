using System;
using System.Collections;
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
    
        [SerializeField] private GameObject[] spellPrefabs;
        [SerializeField] private GameObject startSpell;
        [SerializeField] private Transform projectileParent;

        [HideInInspector] public bool attacking;
    
        private void Start()
        {
            attacking = false;
            playerController = GetComponent<PlayerController>();
            if (amount < 1) amount = 1;
        }

        public void StartAttacking()
        {
            if (!attacking)
            {
                attacking = true;
                StartCoroutine(Attack());
            }
            //else attacking = true; //this happens if the coroutine has started, then player stops attacking, but starts attacking again before the spellcast delay took effect
        }
        private IEnumerator Attack()
        {
            while (attacking)
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
                        var projectileInstance = Instantiate(startSpell, (Vector2)transform.position + 0.5f * castDirection, transform.rotation, projectileParent);
                        var projectileController = projectileInstance.GetComponent<Projectiles.ProjectileController>();
                        projectileController.Initiate(castDirection);
                    }
                }
                else if (amount == 1)
                {
                    var projectileInstance = Instantiate(startSpell, (Vector2)transform.position + 0.5f * direction, transform.rotation, projectileParent);
                    var projectileController = projectileInstance.GetComponent<Projectiles.ProjectileController>();
                    projectileController.Initiate(direction);
                }
                else Debug.LogWarning($"somehow trying to cast invalid amount of spells:{amount}");
                
                yield return new WaitForSeconds(delayBetweenSpells);
                if (!InputManager.Instance.AttackAction.IsPressed()) attacking = false;
            }
        }

        private Vector2 RotateVector(Vector2 direction, float angle)
        {
            angle = 2 * Mathf.PI * angle / 360f; //convert to radians
            float newXDir = direction.x * Mathf.Cos(angle) - direction.y * Mathf.Sin(angle);
            float newYDir = direction.x * Mathf.Sin(angle) + direction.y * Mathf.Cos(angle);
            return new  Vector2(newXDir, newYDir);
        }

        public void SetAmount(string stringAmount)
        {
            Int32.TryParse(stringAmount, out amount);
        }
        public void SetAngle(string stringAngle)
        {
            int intAngle;
            Int32.TryParse(stringAngle, out intAngle);
            angle = intAngle;
        }

        public void SetCooldown(string stringCooldown)
        {
            float.TryParse(stringCooldown, out delayBetweenSpells);
        }
    }
}
