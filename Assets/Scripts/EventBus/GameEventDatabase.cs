using UnityEngine;
// All game events are listed here


/// <summary>
/// Published by the dying enemy, it contains the enemy object
/// and enemy data
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
        if (enemyObject == null)
            return "Enemy object already destroyed";
        else
        {
            return "Event name: " + name + "\n" +
                   "Enemy died at position: " + enemyObject.transform.position;
        }
    }
}

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