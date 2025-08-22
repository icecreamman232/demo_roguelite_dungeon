using SGGames.Scripts.HealthSystem;
using UnityEngine;

namespace SGGames.Scripts.Modules
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
            m_playerHealth.SetInvincibleByDash(true);
        }
    }

}
