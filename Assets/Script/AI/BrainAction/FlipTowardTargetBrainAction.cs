using SGGames.Scripts.AI;

namespace SGGames.Script.AI
{
    public class FlipTowardTargetBrainAction : BrainAction
    {
        public override void DoAction()
        {
            var attackDirection = (m_brain.Target.position - m_brain.Owner.transform.position).normalized;
            m_brain.Owner.FlipSprite(attackDirection);
        }
    }
}
