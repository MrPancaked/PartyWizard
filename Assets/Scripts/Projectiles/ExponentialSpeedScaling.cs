using UnityEngine;

namespace Projectiles
{
    public class ExponentialSpeedScaling : SpeedScaling
    {
        private float exponentialSpeedChange;
        protected override void Awake()
        {
            base.Awake();
            exponentialSpeedChange = Mathf.Pow((spellData.endSpeed /  spellData.startSpeed) - 1, 1 / spellData.maxTimeAlive); // calculate growth rate of exponential
            Debug.Log("Exponential speed growth: " + exponentialSpeedChange);
        }
        public override float SetSpeed()
        {
            Debug.Log("Exponential speed:" + spellData.startSpeed * Mathf.Pow(1f + exponentialSpeedChange, projectileController.timeAlive));
            return spellData.startSpeed * Mathf.Pow(1f + exponentialSpeedChange, projectileController.timeAlive);
        }
    }
}