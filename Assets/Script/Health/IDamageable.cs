using UnityEngine;

namespace SGGames.Scripts.HealthSystem
{
    public interface IDamageable
    {
        void TakeDamage(float damage, GameObject source, float invincibleDuration);
    }
}
