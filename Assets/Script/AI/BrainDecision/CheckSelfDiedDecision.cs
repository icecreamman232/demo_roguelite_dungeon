using SGGames.Script.HealthSystem;
using SGGames.Scripts.AI;
using SGGames.Scripts.Entity;

namespace SGGames.Script.AI
{
    public class CheckSelfDiedDecision : BrainDecision
    {
        private EnemyHealth m_enemyHealth;

        public override void Initialize(EnemyBrain brain)
        {
            m_enemyHealth = brain.Owner.GetComponent<EnemyHealth>();
            base.Initialize(brain);
        }

        public override bool CheckDecision()
        {
            return m_enemyHealth.IsDead;
        }
    }
}

