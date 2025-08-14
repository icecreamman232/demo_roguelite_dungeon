using SGGames.Script.Core;
using SGGames.Script.Managers;
using UnityEngine;

namespace SGGames.Scripts.AI
{
    public class SetPlayerAsTargetAIAction : AIAction
    {
        public override void DoAction()
        {
            var player = ServiceLocator.GetService<LevelManager>().Player.transform;
            m_brain.Target = player;
            SetActionState(Global.ActionState.Completed);
        }
    }
}