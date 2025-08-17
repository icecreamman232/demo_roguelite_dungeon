using UnityEngine;

namespace SGGames.Script.HealthSystem
{
    public interface IDamageable
    {
        void TakeDamage(float damage, GameObject source, float invincibleDuration);
    }
}
