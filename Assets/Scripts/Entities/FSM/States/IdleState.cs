using UnityEngine;

namespace Player.FSM.States
{
    public class IdleState : State
    {
        private float idleDuration;

        public IdleState(float pIdleDuration)
        {
            idleDuration = pIdleDuration;
        }

        public bool IdleOver()
        {
            return Time.time > startTime + idleDuration;
        }
    }
}