using System;
using SGGames.Script.Core;
using SGGames.Script.Entity;
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
        [SerializeField] protected float m_knockBackForce;
        [SerializeField] protected float m_knockDuration;

        public Action OnHit;
        
        protected virtual float GetDamage()
        {
            return Random.Range(m_minDamage, m_maxDamage);
        }

        protected virtual void OnTriggerEnter2D(Collider2D other)
        {
            if (!LayerManager.IsInLayerMask(other.gameObject.layer, m_targetMask)) return;
            
            var health = other.gameObject.GetComponent<Health>();
            var entityMovement = other.gameObject.GetComponent<EntityMovement>();

            if (health)
            {
                HitDamageable(health);
            }
            if (entityMovement && !health.IsDead)
            {
                ApplyKnockBack(entityMovement);
            }
        }

        protected virtual void HitDamageable(Health health)
        {
            health.TakeDamage(GetDamage(),this.gameObject, m_invincibilityDuration);
            OnHit?.Invoke();
        }

        protected virtual void ApplyKnockBack(EntityMovement movement)
        {
            var attackDir = (movement.transform.position - transform.position).normalized;
            movement.ApplyKnockBack(attackDir, m_knockBackForce, m_knockDuration);;
        }
    }
}
