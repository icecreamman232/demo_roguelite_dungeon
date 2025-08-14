using SGGames.Script.Core;
using SGGames.Scripts.AI;

namespace SGGames.Script.AI
{
    public class StartMovingAIAction : AIAction
    {
        public override void DoAction()
        {
            m_brain.Owner.Movement.StartMoving();
            SetActionState(Global.ActionState.InProgress);
            base.DoAction();
        }

        public override void UpdateAction()
        {
            if (m_brain.Owner.Movement.CurrentMovementState == Global.MovementState.Ready)
            {
                SetActionState(Global.ActionState.Completed);
            }
            base.UpdateAction();
        }
    }
}
