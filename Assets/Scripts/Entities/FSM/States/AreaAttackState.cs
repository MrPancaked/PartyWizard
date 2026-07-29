using Player;
using Projectiles;
using UnityEngine;

public class AreaAttackState : State
{
    private AttackController attackController;
    
    private int amount;
    private float angle;
    private float delayBetweenSpells;
    private float attackDuration;
    private ProjectileController spell;
    

    public AreaAttackState(AttackController pAttackController, int pAmount, float pAngle, float pDelayBetweenSpells, float pAttackDuration, ProjectileController pSpell)
    {
        attackController = pAttackController;
        amount = pAmount;
        angle = pAngle;
        delayBetweenSpells = pDelayBetweenSpells;
        attackDuration = pAttackDuration;
        spell = pSpell;
    }
    public override void Enter()
    {
        base.Enter();
        InitiateAttacks();
    }
    private void InitiateAttacks()
    {
        attackController.amount = amount;
        attackController.angle = angle;
        attackController.delayBetweenSpells = delayBetweenSpells;
        attackController.currentSpell = spell;
        attackController.wantsToAttack =  true;
    }

    public override void Exit()
    {
        base.Exit();
        attackController.wantsToAttack = false;
    }

    public bool AttackOver()
    {
        return Time.time > startTime + attackDuration;
    }
}
