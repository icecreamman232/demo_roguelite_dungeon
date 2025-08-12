using SGGames.Script.Core;
using SGGames.Scripts.AI;

namespace SGGames.Script.AI
{
    public class FlipTowardTargetBrainAction : BrainAction
    {
        public override void StartTurnAction()
        {
            if(m_brain.Target == null) return;
            var attackDirection = (m_brain.Target.position - m_brain.Owner.transform.position).normalized;
            m_brain.Owner.FlipSprite(attackDirection);
            SetActionState(Global.ActionState.Completed);
        }
    }
}
