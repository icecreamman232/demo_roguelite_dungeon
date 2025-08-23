using SGGames.Scripts.Abilities;
using SGGames.Scripts.Core;
using SGGames.Scripts.HealthSystem;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SGGames.Scrips.Abilities
{
    public class DamageReflect : AbilityBehavior
    {
        [SerializeField] private GameObject m_owner;
        [SerializeField] private float m_chanceToReflect;
        [SerializeField] private float m_percentDamageReflected;
        [SerializeField] private Health m_health;

        [ContextMenu("Activate")]
        public override void Activate()
        {
            m_health.OnHit += OnTakingDamage;
            base.Activate();
        }

        [ContextMenu("Deactivate")]
        public override void Deactivate()
        {
            m_health.OnHit -= OnTakingDamage;
            base.Deactivate();
        }

        private void OnTakingDamage(OnHitInfo onHitInfo)
        {
            //Prevent reflected damage from self.
            if (onHitInfo.DamageType == Global.DamageType.Reflected && onHitInfo.Source == this.gameObject)
            {
                return;
            }
            if(!CanReflectDamage()) return;
            
            var reflectedDamage = onHitInfo.Damage * MathHelpers.PercentToValue(m_percentDamageReflected);
            var ownerHealth = onHitInfo.Owner.GetComponent<IDamageable>();
            ownerHealth.TakeDamage(Global.DamageType.Reflected, reflectedDamage, this.gameObject, m_health.gameObject, 0);
        }

        private bool CanReflectDamage()
        {
            return Random.Range(0, 100) <= m_chanceToReflect;
        }
    }
}
