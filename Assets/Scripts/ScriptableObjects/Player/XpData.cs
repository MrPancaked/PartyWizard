using UnityEngine;

[CreateAssetMenu(fileName = "XpData", menuName = "Scriptable Objects/XpData")]
public class XpData : ScriptableObject
{
    public int xpCount;
    public int startXp;
    public int xpForLevel;
    public int xpMultiplier;
}
