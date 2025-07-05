using SGGames.Script.HealthSystem;
using SGGames.Scripts.AI;
using SGGames.Scripts.Entity;
using UnityEngine;

namespace SGGames.Script.AI
{
    public class GetHitBrainDecision : BrainDecision
    {
        private bool m_isHit;
        public override void Initialize(EnemyBrain brain)
        {
            var health = brain.Owner.GetComponent<EnemyHealth>();
            health.OnHit += EnemyGetHit;
            
            base.Initialize(brain);
        }

        public override bool CheckDecision()
        {
            return m_isHit;
        }
        
        private void EnemyGetHit(float damage, GameObject source)
        {
            m_isHit = true;
            m_brain.Target = source.transform;
        }

        private void OnDestroy()
        {
            var health = m_brain.Owner.GetComponent<EnemyHealth>();
            health.OnHit -= EnemyGetHit;
        }
    }
}

