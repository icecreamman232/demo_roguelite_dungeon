using SGGames.Scripts.AI;

namespace SGGames.Script.AI
{
    public class HasTargetBrainDecision : BrainDecision
    {
        public override bool CheckDecision()
        {
            return m_brain.Target != null;
        }
    }
}
