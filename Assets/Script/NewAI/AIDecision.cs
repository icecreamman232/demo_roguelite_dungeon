using UnityEngine;

namespace SGGames.Scripts.AI
{
    public class AIDecision : MonoBehaviour
    {
        public string Label;
        protected AIBrain m_brain;

        public virtual void Initialize(AIBrain brain)
        {
            m_brain = brain;
        }
        public virtual bool CheckDecision(){ return false;}
    }
}
