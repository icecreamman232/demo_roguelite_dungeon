using SGGames.Script.Core;
using SGGames.Script.Managers;

namespace SGGames.Scripts.AI
{
    public class SetPlayerAsTargetBrainAction : BrainAction
    {
        public override void StartTurnAction()
        {
            var lvlManager = ServiceLocator.GetService<LevelManager>();
            m_brain.Target = lvlManager.Player.transform;
            
            SetActionState(Global.ActionState.Completed);
        }
    }
}
