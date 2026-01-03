using Player;
using UnityEngine;

public class AttackState : State
{
    private AttackController attackController;
    
    private int amount;
    private float angle;
    private float delayBetweenSpells;
    private float attackDuration;
    
    private float attackStartTime;

    public AttackState(AttackController pAttackController, int pAmount, float pAngle, float pDelayBetweenSpells, float pAttackDuration)
    {
        attackController = pAttackController;
        amount = pAmount;
        angle = pAngle;
        delayBetweenSpells = pDelayBetweenSpells;
        attackDuration = pAttackDuration;
    }
    public override void Enter()
    {
        base.Enter();
        attackStartTime = Time.time;
        InitiateAttacks();
    }
    private void InitiateAttacks()
    {
        attackController.amount = amount;
        attackController.angle = angle;
        attackController.delayBetweenSpells = delayBetweenSpells;
        attackController.wantsToAttack =  true;
    }

    public override void Exit()
    {
        base.Exit();
        attackController.wantsToAttack = false;
    }

    public bool AttackOver()
    {
        return Time.time > attackStartTime + attackDuration;
    }
}
