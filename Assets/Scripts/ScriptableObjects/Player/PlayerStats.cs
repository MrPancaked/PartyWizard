using UnityEngine;
/// <summary>
/// Holds starting stats for the player that the PlayerStatController uses for its initialization
/// Can be used to make the player more or less powerfull at the start of the game
/// </summary>
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
