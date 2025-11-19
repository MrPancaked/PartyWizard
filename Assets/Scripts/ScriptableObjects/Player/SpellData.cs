using UnityEngine;

namespace ScriptableObjects.Player
{
    [CreateAssetMenu(fileName = "SpellData", menuName = "Scriptable Objects/SpellData")]
    public class SpellData : ScriptableObject
    {
        public int damage;
        public int amount;
        public float angleBetweenSpells;
        public bool aoeEffect;
        public int aoeDamage;
        public float aoeRadius;
        public float aoePower;
        public float startSpeed;
        public float maxTimeAlive;
        public bool hurtPlayer;
        public bool hurtEnemy;
        public SpeedScaling speedScaling;
        public DirectionChange directionChange;
        public enum SpeedScaling
        {
            None, LinearInc, LinearDec 
        }
        public enum DirectionChange
        {
            None
        }
    }
}

