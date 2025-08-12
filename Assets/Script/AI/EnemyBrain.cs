using System.Collections.Generic;
using SGGames.Script.Core;
using SGGames.Script.Events;
using SGGames.Scripts.AI;
using UnityEngine;

namespace SGGames.Scripts.Entity
{
    public class EnemyBrain : MonoBehaviour
    {
        [SerializeField] private SwitchTurnEvent m_switchTurnEvent;
        [SerializeField] private List<BrainState> m_states;
        private EnemyController m_owner;
        public Transform Target;
        private bool m_brainActive;
        
        public BrainState CurrentState { get;private set; }
        public float TimeInState { get; private set; }
        public bool IsBrainActive => m_brainActive;
        
        public EnemyController Owner
        {
            get => m_owner;
            set => m_owner = value;
        }

        private void Update()
        {
            if (!m_brainActive) return;

            if (CurrentState != null)
            {
                CurrentState.UpdateActionProgress();
            }
        }

        public void Initialize(EnemyController controller)
        {
            m_owner = controller;
            foreach (var state in m_states)
            {
                state.Initialize(this);
            }
            m_switchTurnEvent.AddListener(OnSwitchTurnEvent);
        }

        public void CleanUp()
        {
            m_switchTurnEvent.RemoveListener(OnSwitchTurnEvent);
        }
        
        public void ActivateBrain(bool isActive)
        {
            m_brainActive = isActive;
        }

        public void ResetBrain()
        {
            CurrentState = m_states[0];
            foreach (var state in m_states)
            {
                foreach (var action in state.Actions)
                {
                    action.ResetAction();
                }
                foreach (var transition in state.Transitions)
                {
                    transition.BrainDecision.OnReset();
                }
            }
        }
        
        public void TransitionToState(string stateName)
        {
            if (CurrentState == null)
            {
                CurrentState = FindState(stateName);
            }
        }

        public BrainState FindState(string stateName)
        {
            foreach (var state in m_states)
            {
                if (state.StateName == stateName)
                {
                    return state;
                }
            }

            if (stateName != "")
            {
                Debug.LogError($"State name {stateName} not found!");
            }

            return null;
        }
        
        public void CompleteTurn()
        {
            m_owner.FinishTurn();
        }
        
        private void ExecuteTurn()
        {
            if (!m_brainActive) return;

            if (CurrentState == null) return;
            
            CurrentState.DoActions();
            
            if (!m_brainActive) return;
            
            CurrentState.CheckTransitions();

            TimeInState += 1;
        }

        private void StartTurn()
        {
            ExecuteTurn();
        }
        
        private void OnSwitchTurnEvent(TurnBaseEventData turnBaseEventData)
        {
            if (turnBaseEventData.TurnBaseState == Global.TurnBaseState.EnemyTakeTurn
                && turnBaseEventData.EntityIndex == m_owner.OrderIndex)
            {
                ResetBrain();
                ActivateBrain(true);
                StartTurn();
            }
        }
        
        #region Editor
        public BrainAction[] GetAttachedActions()
        {
            var actions = GetComponentsInChildren<BrainAction>();
            return actions;
        }

        public BrainDecision[] GetAttachedDecisions()
        {
            var decisions = GetComponentsInChildren<BrainDecision>();
            return decisions;
        }
        
        #endregion
    }
}
