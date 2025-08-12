using SGGames.Script.Core;
using SGGames.Scripts.Entity;
using UnityEngine;

namespace SGGames.Scripts.AI
{
    public abstract class BrainAction : MonoBehaviour
    {
        [SerializeField] protected Global.ActionState m_currentState;
        public string Label;
        protected EnemyBrain m_brain;
        
        public bool IsCompleted => m_currentState == Global.ActionState.Completed;
        
        public virtual void Initialize(EnemyBrain brain)
        {
            m_brain = brain;
        }

        public abstract void StartTurnAction();
        public virtual void UpdateAction(){}

        protected virtual void SetActionState(Global.ActionState state)
        {
            m_currentState = state;
        }

        public virtual void ResetAction()
        {
            SetActionState(Global.ActionState.NotStarted);
        }
    }
}

