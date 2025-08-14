using System;
using UnityEngine;

namespace SGGames.Scripts.AI
{
    [Serializable]
    public class AIState
    {
        public string StateName;
        private AIBrain m_brain;
        [SerializeField] private bool m_consumeTurn;
        [SerializeField] private bool m_actionsCompleted;
        [SerializeField] private AIDecision[] m_decisions;
        [SerializeField] private AIAction[] m_actions;
        
        public bool ConsumeTurn => m_consumeTurn;
        public bool ActionsCompleted => m_actionsCompleted;
        public AIDecision[] Decisions => m_decisions;
        public AIAction[] Actions => m_actions;

        public virtual void Initialize(AIBrain brain)
        {
            m_brain = brain;
            foreach (var action in m_actions)
            {
                action.Initialize(brain);
            }

            foreach (var decision in m_decisions)
            {
                decision.Initialize(brain);
            }
        }

        public bool CheckDecision()
        {
            bool allDecisionsTrue = true;
            foreach (var decision in m_decisions)
            {
                if (!decision.CheckDecision())
                {
                    allDecisionsTrue = false;
                }
            }
            return allDecisionsTrue;
        }
        
        public void DoActions()
        {
            foreach (var action in m_actions)
            {
                action.DoAction();
            }
        }
        
        public void ResetActions()
        {
            foreach (var action in m_actions)
            {
                action.ResetAction();
            }
            m_actionsCompleted = false;
        }

        public void UpdateActionProgress()
        {
            if (m_actionsCompleted) return;
            bool allActionCompleted = true;
            foreach (var action in m_actions)
            {
                if (action.IsProgress)
                {
                    action.UpdateAction();
                }
                if (!action.IsCompleted)
                {
                    allActionCompleted = false;
                }
            }
            
            if (allActionCompleted)
            {
                m_actionsCompleted = true;
                m_brain.CompleteTurn();
                Debug.Log($"Complete Turn in state {StateName}");
            }
        }
    }
}
