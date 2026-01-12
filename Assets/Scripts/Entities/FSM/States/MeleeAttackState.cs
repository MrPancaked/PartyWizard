using UnityEngine;

namespace Player.FSM.States
{
    public class MeleeAttackState : State
    {
        private AttackController attackController;
        private GameObject sword;
    
        private float attackStartTime;
        private float attackDuration;

        public MeleeAttackState(AttackController pAttackController, float pDuration, GameObject pSword)
        {
            attackController = pAttackController;
            attackDuration = pDuration;
            sword = pSword;
        }
        public override void Enter()
        {
            base.Enter();
            attackStartTime = Time.time;
            InitiateAttacks();
        }
        private void InitiateAttacks()
        {
            attackController.amount = 1;
            attackController.delayBetweenSpells = 1;
            attackController.wantsToAttack = true;
            attackController.currentSpell = sword;
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