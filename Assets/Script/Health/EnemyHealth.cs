using System;
using System.Collections;
using SGGames.Script.Modules;
using SGGames.Scripts.Core;
using SGGames.Scripts.UI;
using SGGames.Scripts.Data;
using SGGames.Scripts.Entities;
using UnityEngine;

namespace SGGames.Scripts.HealthSystem
{
    public class EnemyHealth : Health, IRevivable
    {
        [Header("Enemy")] 
        [SerializeField] private FloatingTextEvent m_floatingTextEvent;
        private EnemyHealthBarController m_enemyHealthBar;
        private FillOverlayColorOnSprite m_fillOverlayColorOnSprite;
        private BloodSplashVFX m_bloodSplashVFX;
        private EnemyController m_controller;
        private const float k_SpriteFlickeringFrequency = 0.1f;
     
        protected override void Start()
        {
            base.Start();
            m_enemyHealthBar = GetComponentInChildren<EnemyHealthBarController>();
            m_fillOverlayColorOnSprite = GetComponentInChildren<FillOverlayColorOnSprite>();
            m_bloodSplashVFX = GetComponentInChildren<BloodSplashVFX>();
            UpdateHealthBar();
        }

        public void Initialize(EnemyController controller)
        {
            m_controller = controller;
            m_maxHealth = m_controller.Data.MaxHealth;
            m_currHealth = m_maxHealth;
        }
        
        protected override IEnumerator OnInvincible(float duration)
        {
            m_isInvincible = true;
            
            var timeStop = duration + Time.time;
            
            while (Time.time < timeStop)
            {
                m_fillOverlayColorOnSprite.FillOverlayColor(m_controller.Model,Color.red,1);
                yield return new WaitForSeconds(k_SpriteFlickeringFrequency);
                m_fillOverlayColorOnSprite.FillOverlayColor(m_controller.Model,Color.red,0);
                yield return new WaitForSeconds(k_SpriteFlickeringFrequency);
            }
            
            m_fillOverlayColorOnSprite.FillOverlayColor(m_controller.Model,Color.white,0);
            
            m_isInvincible = false;
        }

        protected override void Damage(Global.DamageType damageType, float damage, GameObject source, GameObject owner)
        {
            base.Damage(damageType, damage, source, owner);
            var attackDirection = (transform.position - source.transform.position).normalized;
            m_bloodSplashVFX.PlayAtDirection(attackDirection);
            
            m_floatingTextEvent.Raise(new FloatingTextData
            {
                Content = damage.ToString(),
                Position = transform.position,
                Color = GetDamageColor(damageType)
            });
        }

        protected override void UpdateHealthBar()
        {
            m_enemyHealthBar.UpdateHealthBar(m_currHealth, m_maxHealth);
            base.UpdateHealthBar();
        }

        public void OnRevive()
        {
            m_isInvincible = false;
            UpdateHealthBar();
        }

        public void SelfKill()
        {
            try
            {
                TakeDamage(Global.DamageType.Normal,m_currHealth, null, this.gameObject,0);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error in TakeDamage for {gameObject.name}: {ex.Message}\nStack Trace: {ex.StackTrace}");
            }
        }
    }
}


