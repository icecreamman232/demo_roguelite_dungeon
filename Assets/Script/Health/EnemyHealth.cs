using SGGames.Script.Data;
using SGGames.Script.UI;
using UnityEngine;

namespace SGGames.Script.HealthSystem
{
    public class EnemyHealth : Health
    {
        [Header("Enemy")] 
        [SerializeField] private EnemyData m_enemyData;
        [SerializeField] private EnemyHealthBarController m_enemyHealthBar;
        
        protected override void Start()
        {
            m_maxHealth = m_enemyData.MaxHealth;
            base.Start();

            m_enemyHealthBar = GetComponentInChildren<EnemyHealthBarController>();
            UpdateHealthBar();
        }

        protected override void UpdateHealthBar()
        {
            m_enemyHealthBar.UpdateHealthBar(m_currHealth, m_maxHealth);
            base.UpdateHealthBar();
        }
    }
}


