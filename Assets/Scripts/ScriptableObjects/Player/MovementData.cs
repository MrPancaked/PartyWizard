using UnityEngine;

namespace ScriptableObjects.Player
{
    [CreateAssetMenu(fileName = "MovementData", menuName = "Scriptable Objects/MovementData")]
    public class MovementData : ScriptableObject
    {
        public float speed;
        public float friction;
    }
}
