using System.Collections;
using SGGames.Script.Core;
using SGGames.Script.Data;
using SGGames.Script.Entity;
using SGGames.Script.Events;
using SGGames.Script.Managers;
using UnityEngine;

namespace SGGames.Script.HealthSystem
{
    public class PlayerHealth : Health
    {
        [SerializeField] private PlayerData m_playerData;
        [SerializeField] private UpdatePlayerHealthEvent m_updatePlayerHealthEvent;
        [SerializeField] private DebugSettings m_debugSettings;
        [SerializeField] private bool m_invincibleByItem;
        
        private PlayerController m_playerController;
        private const float k_FlickeringInterval = 0.1f; 
        
        protected override void Start()
        {
            m_maxHealth = m_playerData.MaxHealth;
            base.Start();
            m_updatePlayerHealthEvent.Raise(m_currHealth, m_maxHealth, isInitialize:true);
        }

        public void SetController(PlayerController controller)
        {
            m_playerController = controller;
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
    }
}
