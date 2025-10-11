using UnityEngine;

[CreateAssetMenu(fileName = "EntitySettings", menuName = "Scriptable Objects/EntitySettings")]
public class EntitySettings : ScriptableObject
{
    [Header("hp settings")]
    public int hp;
    public int maxHp;
}
