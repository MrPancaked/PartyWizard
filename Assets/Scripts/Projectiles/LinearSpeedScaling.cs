using UnityEngine;

namespace Projectiles
{
    public class LinearSpeedScaling : SpeedScaling
    { 
        private float linearSpeedChange;
        protected override void Awake()
        {
            base.Awake();
            linearSpeedChange = (spellData.endSpeed - spellData.startSpeed) / spellData.maxTimeAlive; // calculate change of speed per second based on the total time the projectile will be alive
        }
        public override float SetSpeed()
        {
            Debug.Log("Linear speed: " + linearSpeedChange * projectileController.timeAlive);
            return spellData.startSpeed + linearSpeedChange * projectileController.timeAlive;
        }
    }
}