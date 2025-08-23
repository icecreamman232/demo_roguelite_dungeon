using System;
using System.Collections;
using SGGames.Scripts.Core;
using SGGames.Scripts.Entities;
using UnityEngine;

namespace SGGames.Scripts.HealthSystem
{
    [Serializable]
    public class OnHitInfo
    {
        public float Damage;
        public Global.DamageType DamageType;
        public GameObject Source;
        public GameObject Owner;
    }
    
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
        
        private OnHitInfo m_onHitInfo;
        private ReviveComponent m_reviveComponent;
        private Animator m_animator;
        private int DEAD_TRIGGER_ANIM_PARAM = Animator.StringToHash("Trigger_Dead");

        public float MaxHealth => m_maxHealth;
        public float CurrentHealth => m_currHealth;
        public bool IsInvincible => m_isInvincible;

        public bool IsDead => m_currHealth <= 0;
        public bool CanRevive => m_reviveComponent && m_reviveComponent.CanRevive();

        public Action<OnHitInfo> OnHit;
        public Action OnDeath;
        
        protected virtual void Start()
        {
            m_currHealth = m_maxHealth;
            m_animator = GetComponentInChildren<Animator>();
            m_reviveComponent = GetComponent<ReviveComponent>();
            m_onHitInfo = new OnHitInfo()
            {
                Damage = 0,
                Source = null,
                Owner = null,
            };
        }

        private void OnDestroy()
        {
            OnHit = null;
            OnDeath = null;
        }

        public virtual void TakeDamage(Global.DamageType damageType, float damage, GameObject source, GameObject owner, float invincibleDuration)
        {
            if (!CanTakeDamage()) return;
            
            Damage(damageType, damage, source, owner);

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

        protected virtual void Damage(Global.DamageType damageType, float damage, GameObject source, GameObject owner)
        {
            //Debug.Log($"{this.gameObject.name} take damage {damage} from {source.gameObject.name}");
            m_currHealth -= damage;
            if (m_currHealth <= 0)
            {
                m_currHealth = 0;
            }
            m_onHitInfo.DamageType = damageType;
            m_onHitInfo.Damage = damage;
            m_onHitInfo.Source = source;
            m_onHitInfo.Owner = owner;
            OnHit?.Invoke(m_onHitInfo);
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

        protected Color GetDamageColor(Global.DamageType damageType)
        {
            switch (damageType)
            {
                case Global.DamageType.Normal:
                    return Color.white;
                case Global.DamageType.Reflected:
                    return Color.yellow;
                case Global.DamageType.Poison:
                    return Color.green;
                case Global.DamageType.Burn:
                    return new Color(0.9f, 0.3f, 0.2f);
                case Global.DamageType.Frozen:
                    return Color.cyan;
            }
            return Color.white;
        }
    }
}

