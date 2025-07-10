using SGGames.Script.HealthSystem;
using UnityEngine;

namespace SGGames.Script.Modules
{
    public class EnableInvincibleDashCommand : IDashCommand
    {
        private PlayerHealth m_playerHealth;
        
        public void Initialize(GameObject target)
        {
            m_playerHealth = target.GetComponent<PlayerHealth>();
        }

        public void Execute()
        {
            m_playerHealth.SetInvincible(true);
        }
    }

}
