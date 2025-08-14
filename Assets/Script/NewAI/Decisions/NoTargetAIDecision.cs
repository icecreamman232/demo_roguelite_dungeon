namespace SGGames.Scripts.AI
{
    public class NoTargetAIDecision : AIDecision
    {
        public override bool CheckDecision()
        {
            return m_brain.Target == null;
        }
    }
}
