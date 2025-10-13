using UnityEngine;

[CreateAssetMenu(fileName = "EntitySettings", menuName = "Scriptable Objects/EntitySettings")]
public class EntitySettings : ScriptableObject
{
    [Header("settings")] 
    public int hp;
    public int maxHp;
    public float speed;
}