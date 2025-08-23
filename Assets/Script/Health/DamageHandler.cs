using System;
using SGGames.Scripts.Core;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SGGames.Scripts.HealthSystem
{
    /// <summary>
    /// Base class to handle damage dealt to entity
    /// </summary>
    public class DamageHandler : MonoBehaviour
    {
        [SerializeField] protected Global.DamageType m_damageType = Global.DamageType.Normal;
        [SerializeField] protected LayerMask m_targetMask;
        [SerializeField] protected float m_minDamage;
        [SerializeField] protected float m_maxDamage;
        [SerializeField] protected float m_invincibilityDuration;
        [SerializeField] protected GameObject m_owner;
        
        public GameObject Owner => m_owner;
        public Action OnHit;

        public void SetOwner(GameObject owner)
        {
            m_owner = owner;
        }
        
        protected virtual float GetDamage()
        {
            return Random.Range(m_minDamage, m_maxDamage);
        }

        protected virtual void OnTriggerEnter2D(Collider2D other)
        {
            if (!LayerManager.IsInLayerMask(other.gameObject.layer, m_targetMask)) return;
            
            var damageable = other.gameObject.GetComponent<IDamageable>();
            
            if (damageable != null)
            {
                HitDamageable(damageable);
            }
        }

        protected virtual void HitDamageable(IDamageable damageable)
        {
            if (m_owner == null)
            {
                Debug.LogError($"Damage handler owner is null on {this.gameObject.name}");
            }
            
            damageable.TakeDamage(m_damageType, GetDamage(), m_owner ?? this.gameObject, m_owner, m_invincibilityDuration);
            OnHit?.Invoke();
        }
    }
}
