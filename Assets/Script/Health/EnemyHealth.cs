
using SGGames.Script.Data;
using UnityEngine;

namespace SGGames.Script.HealthSystem
{
    public class EnemyHealth : Health
    {
        [Header("Enemy")] 
        [SerializeField] private EnemyData m_enemyData;

        protected override void Awake()
        {
            m_maxHealth = m_enemyData.MaxHealth;
            base.Awake();
        }
    }
}


