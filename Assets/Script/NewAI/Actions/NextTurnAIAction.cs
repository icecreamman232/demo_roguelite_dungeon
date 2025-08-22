using SGGames.Scripts.Core;

namespace SGGames.Scripts.AI
{
    public class NextTurnAIAction : AIAction
    {
        public override void DoAction()
        {
            SetActionState(Global.ActionState.Completed);
            base.DoAction();
        }
    }
}
