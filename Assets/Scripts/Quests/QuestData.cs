using Quests;using UnityEngine;
/// <summary>
/// Base class for different kinds of quest data
/// holds values for
/// </summary>
public abstract class QuestData : ScriptableObject
{
    public string questName;
    public string description;
    public int amount;
}
