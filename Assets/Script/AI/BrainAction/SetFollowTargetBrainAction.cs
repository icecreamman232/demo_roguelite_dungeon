using SGGames.Script.Core;
using SGGames.Scripts.AI;
using UnityEngine;

namespace SGGames.Script.AI
{
    public class SetFollowTargetBrainAction : BrainAction
    {
        public override void StartTurnAction()
        {
            if (m_brain.Target == null)
            {
                Debug.LogError("Target is null");
                return;
            }
            m_brain.Owner.Movement.SetFollowingTarget(m_brain.Target);
            SetActionState(Global.ActionState.Completed);
        }
        
        public override void DoAction()
        {
            
        }
    }
}
