using SGGames.Scripts.Core;
using UnityEngine;

namespace SGGames.Scripts.HealthSystem
{
    public interface IDamageable
    {
        /// <summary>
        /// Taking damage from an damage source
        /// </summary>
        /// <param name="damage"></param>
        /// <param name="source">Source of damage. Could be weapon, projectile, trap etc...</param>
        /// <param name="owner">The owner of the damage source. Most of the time, it will be the health component on that entity</param>
        /// <param name="invincibleDuration"></param>
        void TakeDamage(Global.DamageType damageType, float damage, GameObject source, GameObject owner, float invincibleDuration);
    }
}
