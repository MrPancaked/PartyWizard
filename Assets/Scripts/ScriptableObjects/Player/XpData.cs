using UnityEngine;

[CreateAssetMenu(fileName = "XpData", menuName = "Scriptable Objects/XpData")]
public class XpData : ScriptableObject
{
    public int xpCount;
    public int xpForLevel = 10;
    public int xpMultiplier = 1;
}
