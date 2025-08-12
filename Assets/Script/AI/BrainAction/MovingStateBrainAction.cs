using SGGames.Script.Core;
using SGGames.Scripts.AI;
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

        public override void StartTurnAction()
        {
            if (m_shouldStart)
            {
                m_brain.Owner.Movement.StartMoving();
                return;
            }

            if (m_shouldStop)
            {
                m_brain.Owner.Movement.StopMoving();
                return;
            }

            if (m_shouldPause)
            {
                m_brain.Owner.Movement.PauseMoving();
            }
            
            SetActionState(Global.ActionState.InProgress);
        }

        public override void UpdateAction()
        {
            if (m_brain.Owner.Movement.CurrentMovementState == Global.MovementState.Ready)
            {
                SetActionState(Global.ActionState.Completed);
                return;
            }
            base.UpdateAction();
        }
    }
}

