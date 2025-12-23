using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "Scriptable Objects/PlayerStats")]
public class PlayerStats : ScriptableObject
{
    [SerializeField] private PlayerStats startingStats;

    [Header("Hp")] 
    public int extraHp;
    [Header("Damage")] 
    public int flatDamage;
    public int damageMultiplier;
    [Header("Knockback")] 
    public int flatKnockback;
    public int knockbackMultiplier;
    [Header("Movement")] 
    public float movementSpeed;
    [Header("Xp")] 
    public int xpMultiplier = 1;
    [Header("Items")]
    public float pickUpRange;
    public int inventorySlots;
    [Header("SpellCast")]
    public float spellCastDelay;
    public int spellCastAmount;
}
