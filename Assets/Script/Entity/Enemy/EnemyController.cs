using SGGames.Script.Entity;
using UnityEngine;

namespace SGGames.Scripts.Entity
{
    public class EnemyController : EntityBehavior
    {
        [SerializeField] protected EnemyBrain m_currentBrain;

        public EnemyBrain CurrentBrain => m_currentBrain;

        private void Start()
        {
            m_currentBrain.BrainActive = true;
        }

        public void SetActiveBrain(EnemyBrain newBrain)
        {
            m_currentBrain = newBrain;
        }
    }
}

