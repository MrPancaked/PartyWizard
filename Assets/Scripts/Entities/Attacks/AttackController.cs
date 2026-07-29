using System;
using System.Collections;
using Projectiles;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    /*
     * AttackController takes care of attacking for the player and enemies
     * It's intended use was for projectiles only, which is why it instantiates a ProjectileController
     * But the ProjectileController also works well for melee attacks too
     */
    public class AttackController : MonoBehaviour
    {
        [Header("Spell amount Settings")]
        public int amount;
        public float angle;
        public float delayBetweenSpells;
    
        [SerializeField] private ProjectileController[] spellPrefabs;
        [SerializeField] private ProjectileController startSpell;
        [SerializeField] private Transform projectileParent;

        [HideInInspector] public ProjectileController currentSpell;
        private bool attacking;
        [HideInInspector] public bool wantsToAttack;
        private bool isPlayer;
        private bool isEnemy;
    
        private void Start()
        {
            currentSpell = startSpell;
            attacking = false;
            if (amount < 1) amount = 1;
            isPlayer = gameObject.CompareTag("Player");
            isEnemy = gameObject.layer == LayerMask.NameToLayer("Enemy");

            if (isPlayer)
            {
                EventBus<SpellCastAmountUpgradeEventData>.OnEventPublished += UpgradeSpellCastAmount;
                EventBus<SpellUpgradeEventData>.OnEventPublished += UpgradeSpell;

                InitializePlayerStats();
            }
                
        }

        private void InitializePlayerStats()
        {
            PlayerStats playerStartingStats = PlayerStatController.Instance.startingStats;
            amount = playerStartingStats.spellCastAmount;
            delayBetweenSpells = playerStartingStats.spellCastDelay;
        }

        private void OnDisable()
        {
            if (isPlayer)
            {
                EventBus<SpellCastAmountUpgradeEventData>.OnEventPublished -= UpgradeSpellCastAmount;
                EventBus<SpellUpgradeEventData>.OnEventPublished -= UpgradeSpell;
            }
        }

        private void Update()
        {
            if (!attacking && wantsToAttack)
            {
                attacking = true;
                StartCoroutine(Attack());
            }
        }
        private IEnumerator Attack()
        {
            while (attacking)
            {
                Vector2 castPosition = gameObject.transform.position;
                Vector2 targetPosition = Vector2.zero;
                if (isPlayer) 
                {
                    if (Camera.main != null) targetPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
                    else Debug.LogWarning("Main Camera not found!");
                }
                else targetPosition = PlayerController.Instance.transform.position; //if its not a player it should target the player
                
                Vector2 direction = (targetPosition - castPosition).normalized;
                
                if (amount > 1)
                {
                    if (angle != 0) direction = RotateVector(direction, -angle / 2f);
                    float angleInterval = angle / (amount - 1); //division by 0 is not a problem because amount is never 1 due to the check earlier
                    for (int i = 0; i < amount; i++)
                    {
                        Vector2 castDirection = RotateVector(direction, angleInterval * i);
                        var projectileInstance = Instantiate(currentSpell, (Vector2)transform.position + 0.5f * castDirection, transform.rotation, projectileParent);
                        projectileInstance.Initiate(castDirection, isPlayer, isEnemy);
                    }
                }
                else if (amount == 1)
                {
                    var projectileInstance = Instantiate(currentSpell, (Vector2)transform.position + 0.5f * direction, transform.rotation, projectileParent);
                    projectileInstance.Initiate(direction, isPlayer, isEnemy);
                }
                else Debug.LogWarning($"somehow trying to cast invalid amount of spells:{amount}");
                
                yield return new WaitForSeconds(delayBetweenSpells);
                if (!wantsToAttack) attacking = false;
            }
        }

        private void UpgradeSpellCastAmount(SpellCastAmountUpgradeEventData eventData)
        {
            amount += eventData.amount;
            angle += 5f;
        }

        private void UpgradeSpell(SpellUpgradeEventData eventData)
        {
            currentSpell = eventData.spellPrefab;
            amount = PlayerStatController.Instance.startingStats.spellCastAmount;
            delayBetweenSpells = PlayerStatController.Instance.startingStats.spellCastDelay;
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
