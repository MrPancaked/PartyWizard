using UnityEngine;

namespace ScriptableObjects.Player
{
    [CreateAssetMenu(fileName = "HpData", menuName = "Scriptable Objects/HpData")]
    public class HpData : ScriptableObject
    {
        public int startHp;
        public int startShield;
    }
}

