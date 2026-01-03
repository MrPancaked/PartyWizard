using UnityEngine;

namespace Player.FSM.States
{
    public class IdleState : State
    {
        private float idleDuration;
        private float idleStartTime;

        public IdleState(float pIdleDuration)
        {
            idleDuration = pIdleDuration;
        }

        public override void Enter()
        {
            base.Enter();
            idleStartTime = Time.time;
        }

        public bool IdleOver()
        {
            return Time.time > idleStartTime + idleDuration;
        }
    }
}