using SGGames.Scripts.Entity;
using UnityEngine;

namespace SGGames.Scripts.AI
{
    public abstract class BrainDecision : MonoBehaviour
    {
        public string Label;
        protected EnemyBrain m_brain;

        public virtual void Initialize(EnemyBrain brain)
        {
            m_brain = brain;
        }
        public virtual void OnEnterState(){}
        public abstract bool CheckDecision();
        public virtual void OnExitState(){}
    }
}

