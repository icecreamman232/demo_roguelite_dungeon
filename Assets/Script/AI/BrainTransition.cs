using System;
using SGGames.Scripts.Entity;


namespace SGGames.Scripts.AI
{
    [Serializable]
    public class BrainTransition
    {
        public BrainDecision BrainDecision;
        public string TrueStateName;
        public string FalseStateName;

        public void Initialize(EnemyBrain brain)
        {
            if (BrainDecision == null) return;
            BrainDecision.Initialize(brain);
        }
        
        public void CheckTransition()
        {
            
        }
    }
}

