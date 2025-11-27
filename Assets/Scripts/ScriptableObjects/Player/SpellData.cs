using UnityEngine;

namespace ScriptableObjects.Player
{
    [CreateAssetMenu(fileName = "SpellData", menuName = "Scriptable Objects/SpellData")]
    public class SpellData : ScriptableObject
    {
        [Header("AOE settings")]
        public bool aoeEffect;
        public int aoeDamage;
        public float aoeRadius;
        public float aoeKnockback;
        [Header("SpeedSettings")]
        public float startSpeed;
        public float endSpeed;
        public float maxTimeAlive;
        public SpeedScaling speedScaling;
        public DirectionChange directionChange;
        public float directionChangeStrength;
        [Header("damage Settings")]
        public float knockback;
        public int damage;
        public bool hurtPlayer;
        public bool hurtEnemy;
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

