using ScriptableObjects.Player;
using UnityEngine;

namespace Projectiles
{
    public abstract class SpeedScaling : MonoBehaviour
    {
        [SerializeField] protected ProjectileController projectileController;
        protected SpellData spellData;

        protected virtual void Awake()
        {
            spellData = projectileController.spellData;
        }
        public abstract float SetSpeed();
    }
}

