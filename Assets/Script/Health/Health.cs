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
        [SerializeField] protected bool m_disableOnDeath = true;
        [SerializeField] protected float m_delayAfterDeath;
        
        private ReviveComponent m_reviveComponent;
        private Animator m_animator;
        private int DEAD_TRIGGER_ANIM_PARAM = Animator.StringToHash("Trigger_Dead");

        public float MaxHealth => m_maxHealth;
        public float CurrentHealth => m_currHealth;
        public bool IsInvincible => m_isInvincible;

        public bool IsDead => m_currHealth <= 0;

        public Action<float, GameObject> OnHit;
        public Action OnDeath;
        
        protected virtual void Start()
        {
            m_currHealth = m_maxHealth;
            m_animator = GetComponentInChildren<Animator>();
            m_reviveComponent = GetComponent<ReviveComponent>();
        }
        
        public virtual void TakeDamage(float damage, GameObject source, float invincibleDuration)
        {
            if (!CanTakeDamage()) return;
            
            Damage(damage, source);

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

        public virtual void SetHealth(float health)
        {
            m_currHealth = health;
        }
        
        protected virtual bool CanTakeDamage()
        {
            if (m_isInvincible) return false;
            
            if(m_currHealth <= 0) return false;
            
            return true;
        }

        protected virtual void Damage(float damage, GameObject source)
        {
            m_currHealth -= damage;
            OnHit?.Invoke(damage, source);
        }

        protected virtual void UpdateHealthBar()
        {
            
        }
        
        protected virtual IEnumerator OnInvincible(float duration)
        {
            m_isInvincible = true;
            yield return new WaitForSeconds(duration);
            m_isInvincible = false;
        }

        protected virtual void Kill()
        {
            StartCoroutine(KillCoroutine());
        }

        protected virtual IEnumerator KillCoroutine()
        {
            OnDeath?.Invoke();
            m_isInvincible = true;
            if (m_animator)
            {
                m_animator.SetTrigger(DEAD_TRIGGER_ANIM_PARAM);
            }
            yield return new WaitForSeconds(m_delayAfterDeath);

            var canRevive = m_reviveComponent.CanRevive();
            
            if (m_reviveComponent)
            {
                m_reviveComponent.Revive();
            }
            
            if (m_disableOnDeath && !canRevive)
            {
                this.gameObject.SetActive(false);
            }
        }
    }
}

