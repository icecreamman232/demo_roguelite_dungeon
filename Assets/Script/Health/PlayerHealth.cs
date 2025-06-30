using SGGames.Script.Data;
using SGGames.Script.Events;
using UnityEngine;

namespace SGGames.Script.HealthSystem
{
    public class PlayerHealth : Health
    {
        [SerializeField] private PlayerData m_playerData;
        [SerializeField] private UpdatePlayerHealthEvent m_updatePlayerHealthEvent;

        protected override void Start()
        {
            m_maxHealth = m_playerData.MaxHealth;
            base.Start();
            m_updatePlayerHealthEvent.Raise(m_currHealth, m_maxHealth,isInitialize:true);
        }

        protected override void UpdateHealthBar()
        {
            m_updatePlayerHealthEvent.Raise(m_currHealth, m_maxHealth, isInitialize:false);
            base.UpdateHealthBar();
        }
    }
}
