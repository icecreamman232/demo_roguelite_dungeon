using SGGames.Script.Entity;
using SGGames.Script.HealthSystem;
using UnityEngine;

namespace SGGames.Scripts.Entity
{
    public class EnemyController : EntityBehavior
    {
        [SerializeField] protected EnemyBrain m_currentBrain;

        private EnemyHealth m_health;

        public EnemyBrain CurrentBrain => m_currentBrain;

        private void Start()
        {
            m_currentBrain.BrainActive = true;
            m_health = GetComponent<EnemyHealth>();
            m_health.OnDeath += OnEnemyDeath;
        }

        private void OnEnemyDeath()
        {
            m_health.OnDeath -= OnEnemyDeath;
            m_currentBrain.BrainActive = false;
        }

        public void SetActiveBrain(EnemyBrain newBrain)
        {
            m_currentBrain = newBrain;
        }
    }
}

