using SGGames.Script.Data;
using UnityEngine;

namespace SGGames.Script.HealthSystem
{
    public class PlayerHealth : Health
    {
        [SerializeField] private PlayerData m_playerData;

        protected override void Awake()
        {
            m_maxHealth = m_playerData.MaxHealth;
            base.Awake();
        }
    }
}
