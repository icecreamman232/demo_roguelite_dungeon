using System;
using System.Collections;
using SGGames.Script.Entity;
using UnityEngine;


namespace SGGames.Script.HealthSystem
{
    /// <summary>
    /// Base class to compute health for entity
    /// </summary>
    public class Health : EntityBehavior
    {
        [SerializeField] protected float m_maxHealth;
        [SerializeField] protected float m_currHealth;
        [SerializeField] protected bool m_isInvincible;

        public float MaxHealth => m_maxHealth;
        public float CurrentHealth => m_currHealth;
        public bool IsInvincible => m_isInvincible;

        public Action OnDeath;
        
        protected virtual void Start()
        {
            m_currHealth = m_maxHealth;
        }
        
        public virtual void TakeDamage(float damage, GameObject source, float invincibleDuration)
        {
            if (!CanTakeDamage()) return;
            
            m_currHealth -= damage;

            UpdateHealthBar();
            
            if (m_currHealth <= 0)
            {
                Kill();
            }
            else
            {
                StartCoroutine(OnInvincible(invincibleDuration));
            }
        }

        protected virtual void UpdateHealthBar()
        {
            
        }
        
        protected virtual bool CanTakeDamage()
        {
            if (m_isInvincible) return false;
            
            if(m_currHealth <= 0) return false;
            
            return true;
        }

        protected virtual IEnumerator OnInvincible(float duration)
        {
            m_isInvincible = true;
            yield return new WaitForSeconds(duration);
            m_isInvincible = false;
        }

        protected virtual void Kill()
        {
            OnDeath?.Invoke();
            m_isInvincible = true;
            this.gameObject.SetActive(false);
        }

        protected virtual void OnDestroy()
        {
            
        }
    }
}

