using SGGames.Script.Data;
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

        protected override void Start()
        {
            m_maxHealth = m_playerData.MaxHealth;
            base.Start();
            m_updatePlayerHealthEvent.Raise(m_currHealth, m_maxHealth,isInitialize:true);
        }

        protected override bool CanTakeDamage()
        {
            if (m_debugSettings.IsPlayerImmortal) return false;
            
            return base.CanTakeDamage();
        }

        protected override void UpdateHealthBar()
        {
            m_updatePlayerHealthEvent.Raise(m_currHealth, m_maxHealth, isInitialize:false);
            base.UpdateHealthBar();
        }

        public void KillPlayer()
        {
            StopAllCoroutines();
            Kill();
        }
    }
}
