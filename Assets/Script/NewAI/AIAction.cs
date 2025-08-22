using SGGames.Scripts.Core;
using UnityEngine;

namespace SGGames.Scripts.AI
{
    public class AIAction : MonoBehaviour
    {
        public string Label;
        [SerializeField] protected Global.ActionState m_currentState;
        protected AIBrain m_brain;
        
        public bool IsCompleted => m_currentState == Global.ActionState.Completed;
        public bool IsProgress => m_currentState == Global.ActionState.InProgress;
        public virtual void Initialize(AIBrain brain)
        {
            m_brain = brain;
        }
        public virtual void DoAction(){}
        public virtual void UpdateAction(){}

        public void SetActionState(Global.ActionState state)
        {
            m_currentState = state;
        }

        public void ResetAction()
        {
            SetActionState(Global.ActionState.NotStarted);
        }
    }
}
