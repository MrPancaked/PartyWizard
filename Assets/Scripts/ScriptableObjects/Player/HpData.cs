using UnityEngine;

namespace ScriptableObjects.Player
{
    [CreateAssetMenu(fileName = "HpData", menuName = "Scriptable Objects/HpData")]
    public class HpData : ScriptableObject
    {
        public int startHp;
        public int maxHp;
        public int startShield;
        public int contactDamage;
        public float contactKnockback;
        public bool takeDamage;
        public ParticleSystem destroyEffectObject;
    }
}

