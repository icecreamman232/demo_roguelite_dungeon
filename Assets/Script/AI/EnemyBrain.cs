using System;
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
        public BrainState CurrentState;

        public float TimeInState;
        
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
            if (m_brainActive && CurrentState != null)
            {
                CurrentState.EnterState();
            }
        }

        public void ResetBrain()
        {
            CurrentState = m_states[0];
            foreach (var state in m_states)
            {
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
                if (CurrentState != null)
                {
                    CurrentState.EnterState();
                }

                return;
            }

            if (stateName != CurrentState.StateName)
            {
                CurrentState.ExitState();
                TimeInState = 0;
                CurrentState = FindState(stateName);
                if (CurrentState != null)
                {
                    CurrentState.EnterState();
                }
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
            Debug.Log("Start Turn");
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
