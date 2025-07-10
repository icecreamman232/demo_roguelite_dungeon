using SGGames.Script.Entity;
using SGGames.Scripts.AI;
using SGGames.Scripts.Entity;
using UnityEngine;

namespace SGGames.Script.AI
{
    public class SetFollowTargetBrainAction : BrainAction
    {
        private EnemyMovement m_movement;

        public override void Initialize(EnemyBrain brain)
        {
            m_movement = brain.Owner.GetComponent<EnemyMovement>();
            base.Initialize(brain);
        }

        public override void OnEnterState()
        {
            if (m_brain.Target == null)
            {
                Debug.LogError("Target is null");
                return;
            }
            m_movement.SetFollowingTarget(m_brain.Target);
            base.OnEnterState();
        }

        public override void DoAction()
        {
            
        }
    }
}
