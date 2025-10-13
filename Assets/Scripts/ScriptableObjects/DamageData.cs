using UnityEngine;

[CreateAssetMenu(fileName = "DamageData", menuName = "Scriptable Objects/DamageData")]
public class DamageData : ScriptableObject
{
    public int damage { get; private set; }
}
