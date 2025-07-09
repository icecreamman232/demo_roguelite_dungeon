using SGGames.Script.Entity;
using SGGames.Scripts.AI;
using SGGames.Scripts.Entity;
using UnityEngine;

namespace SGGames.Script.AI
{
    /// <summary>
    /// Trigger a movement state on enemy when entering a state
    /// </summary>
    public class MovingStateBrainAction : BrainAction
    {
        [SerializeField] private bool m_shouldStart;
        [SerializeField] private bool m_shouldStop;
        [SerializeField] private bool m_shouldPause;
        
        private EnemyMovement m_movement;

        public override void Initialize(EnemyBrain brain)
        {
            m_movement = brain.Owner.gameObject.GetComponent<EnemyMovement>();
            base.Initialize(brain);
        }

        public override void OnEnterState()
        {
            if (m_shouldStart)
            {
                m_movement.StartMoving();
                return;
            }

            if (m_shouldStop)
            {
                Debug.Log($"{m_brain.name} Stop moving");
                m_movement.StopMoving();
                return;
            }

            if (m_shouldPause)
            {
                m_movement.PauseMoving();
            }
            base.OnEnterState();
        }

        public override void DoAction()
        {
            
        }
    }
}

