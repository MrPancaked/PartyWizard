using UnityEngine;

namespace ScriptableObjects.Player
{
    /// <summary>
    /// Hold values that the MovementController uses for its initialization
    /// can be used to make the player or enemies faster or slower, or act more like they are on a slippery floor / floating or on a floor with more friction
    /// </summary>
    [CreateAssetMenu(fileName = "MovementData", menuName = "Scriptable Objects/MovementData")]
    public class MovementData : ScriptableObject
    {
        public float speed;
        public float friction;
    }
}
