using System;
using System.Collections;
using SGGames.Scripts.Managers;
using SGGames.Scripts.Core;
using SGGames.Scripts.Data;
using SGGames.Scripts.Entities;
using SGGames.Scripts.Events;
using SGGames.Scripts.Items;
using UnityEngine;

namespace SGGames.Scripts.HealthSystem
{
    public class PlayerHealth : Health
    {
        [Header("Events")]
        [SerializeField] private WorldEvent m_worldEvent;
        [SerializeField] private GameEvent m_gameEvent;
        [SerializeField] private UpdatePlayerHealthEvent m_updatePlayerHealthEvent;
        [SerializeField] private FloatingTextEvent m_floatingTextEvent;
        [Header("Invincibility Settings")]
        [SerializeField] private bool m_invincibleByItem;
        [SerializeField] private bool m_invincibleByDash;
        [Header("Others")]
        [SerializeField] private PlayerData m_playerData;
        [SerializeField] private DebugSettings m_debugSettings;
        
        private PlayerController m_playerController;
        private Action OnWeaponComboInterrupted;
        private const float k_FlickeringInterval = 0.1f; 
        
        private float CurrentDamageResistance => m_playerController.ResistanceController.CurrentDamageResistance;
        private bool CanDodgeThisAttack => m_playerController.PlayerDodge.CanDodgeThisAttack();
        
        protected override void Start()
        {
            m_maxHealth = m_playerData.MaxHealth;
            base.Start();
            m_updatePlayerHealthEvent.Raise(new UpdatePlayerHealthEventData
            {
                CurrentHealth = m_currHealth,
                MaxHealth = m_maxHealth,
                IsInitialize = true
            });
        }

        public void Initialize(PlayerController controller, Action weaponInterruptedCallback)
        {
            m_playerController = controller;
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

        public void SetInvincibleByDash(bool isInvincible)
        {
            m_invincibleByDash = isInvincible;
        }

        protected override void Damage(Global.DamageType damageType, float damage, GameObject source, GameObject owner)
        {
            var finalDamage = Mathf.RoundToInt(damage * (1 - MathHelpers.PercentToValue(CurrentDamageResistance)));
            OnWeaponComboInterrupted?.Invoke();
            base.Damage(damageType, finalDamage, source, owner);
            m_floatingTextEvent.Raise(new FloatingTextData
            {
                Content = finalDamage.ToString(),
                Position = transform.position
            });
        }

        protected override bool CanTakeDamage()
        {
            if (m_debugSettings.IsPlayerImmortal) return false;
            
            if (m_invincibleByItem) return false;

            if (m_invincibleByDash)
            {
                m_worldEvent.Raise(Global.WorldEventType.OnPlayerPerfectDodge, this.gameObject, null);
                return false;
            }
            
            if (m_isInvincible) return false;

            //if (CanDodgeThisAttack) return false;
            
            if(m_currHealth <= 0) return false;
            
            return true;
        }

        protected override void UpdateHealthBar()
        {
            m_updatePlayerHealthEvent.Raise(new UpdatePlayerHealthEventData
            {
                CurrentHealth = m_currHealth,
                MaxHealth = m_maxHealth,
                IsInitialize = false
            });
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
