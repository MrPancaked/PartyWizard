using Projectiles;
using UnityEngine;

namespace Player.FSM.States
{
    public class MeleeAttackState : State
    {
        private AttackController attackController;
        private ProjectileController swordObject;
        private float attackDuration;

        public MeleeAttackState(AttackController pAttackController, float pDuration, ProjectileController pSwordObject)
        {
            attackController = pAttackController;
            attackDuration = pDuration;
            swordObject = pSwordObject;
        }
        public override void Enter()
        {
            base.Enter();
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
            return Time.time > startTime + attackDuration;
        }
    }
}