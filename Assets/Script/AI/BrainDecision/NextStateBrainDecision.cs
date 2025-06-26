using SGGames.Scripts.AI;

namespace SGGames.Script.AI
{
    /// <summary>
    /// Default set to be true for quick decision
    /// </summary>
    public class NextStateBrainDecision : BrainDecision
    {
        public override bool CheckDecision()
        {
            return true;
        }
    }
}

