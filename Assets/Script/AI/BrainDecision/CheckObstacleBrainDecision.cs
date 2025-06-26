using SGGames.Script.Entity;
using SGGames.Scripts.AI;
using SGGames.Scripts.Entity;
using UnityEngine;

namespace SGGames.Script.AI
{
    public class CheckObstacleBrainDecision : BrainDecision
    {
        [SerializeField] private float m_raycastDistance = 0.15f;
        [SerializeField] private LayerMask m_obstacleMask;
        private EnemyMovement m_movement;
        private RaycastHit2D m_result;
        private BoxCollider2D m_collider2D;

        public override void Initialize(EnemyBrain brain)
        {
            m_collider2D = brain.Owner.gameObject.GetComponent<BoxCollider2D>();
            m_movement = brain.Owner.gameObject.GetComponent<EnemyMovement>();
            base.Initialize(brain);
        }

        public override bool CheckDecision()
        {
            m_result = Physics2D.BoxCast(m_brain.Owner.transform.position,m_collider2D.size, 0,m_movement.MoveDirection,m_raycastDistance,m_obstacleMask);    
            return m_result.collider != null;
        }
    }
}

