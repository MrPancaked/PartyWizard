using System;
using UnityEngine;

public abstract class PlayerObserver : MonoBehaviour
{
    [SerializeField] protected PlayerDamage playerDamage;

    protected void OnEnable()
    {
        playerDamage.onHit += OnPlayerHit;
    }

    protected void OnDisable()
    {
        playerDamage.onHit -= OnPlayerHit;
    }

    //use in inhereted classes to make different things happend on player hit
    //preferably I would pass both damage data and enemy data like velocity to determine knock-back direction but idk how
    //to do that for all the different types of enemies (have a place where general enemydata like that is stored?)
    protected abstract void OnPlayerHit(DamageData damageData);
}