using SGGames.Scripts.Core;
using UnityEngine;

namespace SGGames.Scripts.AI
{
    public class FollowingTargetAIAction : AIAction
    {
        public override void DoAction()
        {
            if (m_brain.Target == null)
            {
                Debug.LogError("Target is null");
                SetActionState(Global.ActionState.Completed);
                return;
            }
            
            m_brain.Owner.Movement.SetFollowingTarget(m_brain.Target);
            SetActionState(Global.ActionState.InProgress);
            base.DoAction();
        }

        public override void UpdateAction()
        {
            if (m_brain.Owner.Movement.CurrentMovementState == Global.MovementState.Ready)
            {
                SetActionState(Global.ActionState.Completed);
            }
        }
    }
}
