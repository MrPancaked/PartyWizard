using UnityEngine;

namespace Player.FSM.States
{
    public class RangedAttackState : State
    {
        private AttackController attackController;
    
        private int amount;
        private float angle;
        private float delayBetweenSpells;
        private float attackDuration;
        private GameObject spell;
    
        private float attackStartTime;

        public RangedAttackState(AttackController pAttackController, int pAmount, float pAngle, float pDelayBetweenSpells, float pAttackDuration, GameObject pSpell)
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
            attackStartTime = Time.time;
            InitiateAttacks();
        }
        private void InitiateAttacks()
        {
            attackController.amount = amount;
            attackController.angle = angle;
            attackController.delayBetweenSpells = delayBetweenSpells;
            attackController.wantsToAttack =  true;
            attackController.currentSpell =  spell;
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
}