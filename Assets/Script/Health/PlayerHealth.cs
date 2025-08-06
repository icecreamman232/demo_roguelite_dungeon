using System;
using System.Collections;
using SGGames.Script.Core;
using SGGames.Script.Data;
using SGGames.Script.Entities;
using SGGames.Script.Entity;
using SGGames.Script.Events;
using SGGames.Script.Managers;
using UnityEngine;

namespace SGGames.Script.HealthSystem
{
    public class PlayerHealth : Health
    {
        [SerializeField] private PlayerData m_playerData;
        [SerializeField] private GameEvent m_gameEvent;
        [SerializeField] private UpdatePlayerHealthEvent m_updatePlayerHealthEvent;
        [SerializeField] private DebugSettings m_debugSettings;
        [SerializeField] private bool m_invincibleByItem;

        private PlayerResistanceController m_resistanceController;
        private PlayerController m_playerController;
        private Action OnWeaponComboInterrupted;
        private const float k_FlickeringInterval = 0.1f; 
        
        protected override void Start()
        {
            m_maxHealth = m_playerData.MaxHealth;
            base.Start();
            m_updatePlayerHealthEvent.Raise(m_currHealth, m_maxHealth, isInitialize:true);
        }

        public void Initialize(PlayerController controller, PlayerResistanceController resistanceController, Action weaponInterruptedCallback)
        {
            m_playerController = controller;
            m_resistanceController = resistanceController;
            OnWeaponComboInterrupted = weaponInterruptedCallback;
        }
        
        public void KillPlayer()
        {
            StopAllCoroutines();
            Kill();
        }

        public void SetInvincible(bool isInvincible)
        {
            m_isInvincible = isInvincible;
        }

        public void SetInvincibleByItem(bool isInvincible)
        {
            m_invincibleByItem = isInvincible;
        }

        protected override void Damage(float damage, GameObject source)
        {
            var finalDamage = damage * (1 - MathHelpers.PercentToValue(m_resistanceController.CurrentDamageResistance));
            m_currHealth -= finalDamage; 
            OnHit?.Invoke(damage, source);
            OnWeaponComboInterrupted?.Invoke();
        }

        protected override bool CanTakeDamage()
        {
            if (m_debugSettings.IsPlayerImmortal) return false;
            
            if (m_invincibleByItem) return false;
            
            return base.CanTakeDamage();
        }

        protected override void UpdateHealthBar()
        {
            m_updatePlayerHealthEvent.Raise(m_currHealth, m_maxHealth, isInitialize:false);
            base.UpdateHealthBar();
        }

        protected override IEnumerator OnInvincible(float duration)
        {
            m_isInvincible = true;
            var timeStop = duration + Time.time;
            var color = m_playerController.SpriteRenderer.color;
            while (Time.time < timeStop)
            {
                m_playerController.SpriteRenderer.color = m_playerController.SpriteRenderer.color.Alpha0();
                yield return new WaitForSeconds(k_FlickeringInterval);
                m_playerController.SpriteRenderer.color = m_playerController.SpriteRenderer.color.Alpha1();
                yield return new WaitForSeconds(k_FlickeringInterval);
            }
            
            m_playerController.SpriteRenderer.color.Alpha1();
            m_isInvincible = false;
        }
        
        protected override void Kill()
        {
            base.Kill();
            m_gameEvent.Raise(Global.GameEventType.GameOver);
        }
    }
}
