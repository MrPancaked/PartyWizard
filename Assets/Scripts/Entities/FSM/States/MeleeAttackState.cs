using UnityEngine;

namespace Player.FSM.States
{
    public class MeleeAttackState : State
    {
        private AttackController attackController;
        private GameObject swordObject;
    
        private float attackStartTime;
        private float attackDuration;

        public MeleeAttackState(AttackController pAttackController, float pDuration, GameObject pSwordObject)
        {
            attackController = pAttackController;
            attackDuration = pDuration;
            swordObject = pSwordObject;
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
            attackController.currentSpell = swordObject;
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