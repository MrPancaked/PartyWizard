using UnityEngine;

namespace ScriptableObjects.Player
{
    /// <summary>
    /// Holds values that are used by ProjectileController for its initialization
    /// can be used to generate various amounts of spell / projectile / hurtobject behavior
    /// is even used for the Knights sword attack, where the start and end speed are set to 0
    /// </summary>
    [CreateAssetMenu(fileName = "SpellData", menuName = "Scriptable Objects/SpellData")]
    public class SpellData : ScriptableObject
    {
        [Header("Melee settings")]
        public bool melee;
        [Header("AOE settings")]
        public bool aoeEffect;
        public int aoeDamage;
        public float aoeRadius;
        public float aoeKnockback;
        [Header("SpeedSettings")]
        public float startSpeed;
        public float endSpeed;
        public float maxTimeAlive;
        public DirectionChange directionChange;
        public float directionChangeStrength;
        [Header("damage Settings")]
        public float knockback;
        public int damage;
        public bool hurtProjectile;
        public enum SpeedScaling
        {
            None, Linear
        }
        public enum DirectionChange
        {
            None, RandomCurve, RandomWiggle
        }
    }
}

