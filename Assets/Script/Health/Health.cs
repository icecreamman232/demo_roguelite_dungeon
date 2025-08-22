using System;
using System.Collections;
using SGGames.Script.Entity;
using UnityEngine;

namespace SGGames.Script.HealthSystem
{
    /// <summary>
    /// Base class to compute health for entity
    /// </summary>
    public class Health : EntityBehavior, IDamageable
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
        public bool CanRevive => m_reviveComponent && m_reviveComponent.CanRevive();

        public Action<float, GameObject> OnHit;
        public Action OnDeath;
        
        protected virtual void Start()
        {
            m_currHealth = m_maxHealth;
            m_animator = GetComponentInChildren<Animator>();
            m_reviveComponent = GetComponent<ReviveComponent>();
        }

        private void OnDestroy()
        {
            OnHit = null;
            OnDeath = null;
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

        public virtual void Heal(float healAmount)
        {
            m_currHealth += healAmount;
            if (m_currHealth > m_maxHealth)
            {
                m_currHealth = m_maxHealth;
            }
            UpdateHealthBar();
            Debug.Log($"Heal {healAmount} on {this.gameObject.name}");
        }
        
        protected virtual bool CanTakeDamage()
        {
            if (m_isInvincible) return false;
            
            if(m_currHealth <= 0) return false;
            
            return true;
        }

        protected virtual void Damage(float damage, GameObject source)
        {
            //Debug.Log($"{this.gameObject.name} take damage {damage} from {source.gameObject.name}");
            m_currHealth -= damage;
            if (m_currHealth <= 0)
            {
                m_currHealth = 0;
            }
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

            if (m_reviveComponent)
            {
                m_reviveComponent.Revive();
            }
            
            if (m_disableOnDeath && !CanRevive)
            {
                this.gameObject.SetActive(false);
            }
        }
    }
}

