using UnityEngine;

namespace ScriptableObjects.Player
{
    /// <summary>
    /// Holds player or enemy values that the HpController uses for its initialization
    /// can be used to give the player or enemies different amounts of hp and other hp / damge related stats
    /// </summary>
    [CreateAssetMenu(fileName = "HpData", menuName = "Scriptable Objects/HpData")]
    public class HpData : ScriptableObject
    {
        public int startHp;
        public int maxHp;
        public float immuneTime;
        public int startShield;
        public int contactDamage;
        public float contactKnockback;
        public bool takeDamage;
        public ParticleSystem destroyEffectObject;
    }
}

