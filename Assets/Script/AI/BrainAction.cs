using SGGames.Scripts.Entity;
using UnityEngine;

namespace SGGames.Scripts.AI
{
    public abstract class BrainAction : MonoBehaviour
    {
        public string Label;
        protected EnemyBrain m_brain;
        
        public virtual void Initialize(EnemyBrain brain)
        {
            m_brain = brain;
        }
        public virtual void OnEnterState(){}
        public abstract void DoAction();
        public virtual void OnExitState(){}
    }
}

