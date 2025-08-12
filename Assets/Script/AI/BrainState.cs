using System;
using System.Collections.Generic;
using SGGames.Scripts.Entity;
using UnityEngine;

namespace SGGames.Scripts.AI
{
    [Serializable]
    public class BrainState
    {
        public string StateName;
        public List<BrainAction> Actions;
        public List<BrainTransition> Transitions;

        private EnemyBrain m_brain;
        private bool m_actionsCompleted = false;
        
        public void Initialize(EnemyBrain brain)
        {
            m_brain = brain;
            
            foreach (var action in Actions)
            {
                if (action == null)
                {
                    continue;
                }
                action.Initialize(brain);
            }

            foreach (var transition in Transitions)
            {
                transition?.Initialize(brain);
            }
        }

        public void DoActions()
        {
            if (Actions.Count == 0)
            {
                m_actionsCompleted = true;
                CheckTransitionsAndComplete();
                return;
            }

            m_actionsCompleted = false;

            foreach (var action in Actions)
            {
                if (action != null)
                {
                    action.StartTurnAction();
                }
                else
                {
                    Debug.LogError($"An action in {this.m_brain.gameObject.name} on state {StateName} is null");
                }
            }
        }

        public void UpdateActionProgress()
        {
            if (m_actionsCompleted) return;
            bool allActionsCompleted = true;
            foreach (var action in Actions)
            {
                action.UpdateAction();
                if (!action.IsCompleted)
                {
                    allActionsCompleted = false;
                }
            }

            if (allActionsCompleted)
            {
                m_actionsCompleted = true;
                CheckTransitionsAndComplete();
            }
        }
        
        private void CheckTransitionsAndComplete()
        {
            // Check for state transitions after actions complete
            CheckTransitions();
        
            // If no transition occurred, complete the turn
            if (m_brain.CurrentState == this)
            {
                m_brain.CompleteTurn();
            }
        }


        public void CheckTransitions()
        {
            if (Transitions.Count == 0) return;

            foreach (var transition in Transitions)
            {
                if (transition.BrainDecision != null)
                {
                    if (transition.BrainDecision.CheckDecision())
                    {
                        if (!string.IsNullOrEmpty(transition.TrueStateName))
                        {
                            m_brain.TransitionToState(transition.TrueStateName);
                            break;
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(transition.FalseStateName))
                        {
                            m_brain.TransitionToState(transition.FalseStateName);
                            break;
                        }
                    }
                }
            }
        }
    }
}

