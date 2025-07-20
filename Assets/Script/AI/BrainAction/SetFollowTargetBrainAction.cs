using SGGames.Script.Entity;
using SGGames.Scripts.AI;
using SGGames.Scripts.Entity;
using UnityEngine;

namespace SGGames.Script.AI
{
    public class SetFollowTargetBrainAction : BrainAction
    {

        public override void OnEnterState()
        {
            if (m_brain.Target == null)
            {
                Debug.LogError("Target is null");
                return;
            }
            m_brain.Owner.Movement.SetFollowingTarget(m_brain.Target);
            base.OnEnterState();
        }

        public override void DoAction()
        {
            
        }
    }
}
