using SGGames.Script.HealthSystem;
using SGGames.Script.Weapons;
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
            if (source.CompareTag("Player"))
            {
                m_brain.Target = source.transform;
            }
            else if (source.CompareTag("PlayerProjectile"))
            {
                var playerProjectile = source.GetComponent<PlayerProjectile>();
                m_brain.Target = playerProjectile.Owner.transform;
            }
        }

        private void OnDestroy()
        {
            var health = m_brain.Owner.GetComponent<EnemyHealth>();
            health.OnHit -= EnemyGetHit;
        }
    }
}

