using UnityEngine;

/// <summary>
/// Holds player or enemy values that the LevelController uses for its initialization
/// can be used to determine the amount of xp that is needed to level up.
/// </summary>
[CreateAssetMenu(fileName = "XpData", menuName = "Scriptable Objects/XpData")]
public class XpData : ScriptableObject
{
    public int xpCount;
    public int xpForLevel = 10;
    public int xpMultiplier = 1;
}
