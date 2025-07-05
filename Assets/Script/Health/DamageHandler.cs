using System;
using SGGames.Script.Core;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SGGames.Script.HealthSystem
{
    /// <summary>
    /// Base class to handle damage dealt to entity
    /// </summary>
    public class DamageHandler : MonoBehaviour
    {
        [SerializeField] protected LayerMask m_targetMask;
        [SerializeField] protected float m_minDamage;
        [SerializeField] protected float m_maxDamage;
        [SerializeField] protected float m_invincibilityDuration;

        public Action OnHit;
        
        protected float GetDamage()
        {
            return Random.Range(m_minDamage, m_maxDamage);
        }

        protected virtual void OnTriggerEnter2D(Collider2D other)
        {
            if (!LayerManager.IsInLayerMask(other.gameObject.layer, m_targetMask)) return;
            
            var health = other.gameObject.GetComponent<Health>();

            if (health == null) return;
            HitDamageable(health);
        }

        protected virtual void HitDamageable(Health health)
        {
            health.TakeDamage(GetDamage(),this.gameObject, m_invincibilityDuration);
            OnHit?.Invoke();
        }
    }
}
