using UnityEngine;
// All game events are listed here


/// <summary>
/// Published by the dying enemy, it contains the enemy object
/// and enemy data
/// updates quests and enemy count in the gamemanager
/// </summary>
public class EnemyDieEventData : EventData
{
    public GameObject enemyObject;
    public EnemyDieEventData(GameObject pEnemyObject)
    {
        name = "EnemyDieEvent";
        enemyObject = pEnemyObject;
    }

    //Overriding ToString method to display event information for debugging
    public override string ToString()
    {
        base.ToString();
        if (enemyObject == null)
            return "Enemy object already destroyed";
        else
        {
            return "Event name: " + name + "\n" +
                   "Enemy died at position: " + enemyObject.transform.position;
        }
    }
}
/// <summary>
/// updates game state in the game manager (death screen)
/// </summary>
public class PlayerDieEventData : EventData
{
    public GameObject playerObject;

    public PlayerDieEventData(GameObject pPlayerObject)
    {
        name = "PlayerDieEvent";
        playerObject = pPlayerObject;
    }
    
    //Overriding ToString method to display event information for debugging
    public override string ToString()
    {
        if (playerObject == null)
            return "Player object already destroyed";
        else
        {
            return "Event name: " + name + "\n" +
                   "Player died at position: " + playerObject.transform.position;
        }
    }
}
/// <summary>
/// Updates enemy count in the game manager
/// </summary>
public class EnemySpawnEventData : EventData
{
    public GameObject enemyObject;

    public EnemySpawnEventData(GameObject pEnemyObject)
    {
        name = "EnemySpawnEvent";
        enemyObject = pEnemyObject;
    }
    public override string ToString()
    {
        if (enemyObject == null)
            return "Enemy Object already destroyed";
        else
        {
            return "Event name: " + name + "\n" +
                   "Enemy Spawned: " + enemyObject.transform.position;
        }
    }
}
/// <summary>
/// updates inventory and quests
/// </summary>
public class UseItemEvent : EventData
{
    public Item item;
    public UseItemEvent(Item pItem)
    {
        name = "UseItemEvent";
        item = pItem;
    }
}
/// <summary>
/// closes upgrade menu and serves as a base game event class for specific upgrades
/// </summary>
public class PlayerUpgradeEventData : EventData
{
    public Upgrade upgrade;
    public PlayerUpgradeEventData()
    {
        name = "PlayerUpgradeEvent";
    }
}
/// <summary>
/// updates max hp in the players HpController
/// </summary>
public class ExtraHpUpgradeEventData : PlayerUpgradeEventData
{
    public ExtraHpUpgradeEventData(ExtraHpUpgrade pUpgrade)
    {
        upgrade = pUpgrade;
        name = "ExtraHpUpgradeEvent";
    }
}