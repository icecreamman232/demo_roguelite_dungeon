using SGGames.Scripts.AI;

namespace SGGames.Script.AI
{
    public class CheckSelfDiedDecision : BrainDecision
    {
        
        public override bool CheckDecision()
        {
            return m_brain.Owner.Health.IsDead;
        }
    }
}

