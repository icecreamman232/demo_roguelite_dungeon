using System.Collections.Generic;
using SGGames.Scripts.AI;
using UnityEngine;

namespace SGGames.Scripts.Entity
{
    public class EnemyBrain : MonoBehaviour
    {
        [SerializeField] private List<BrainState> States;
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

        private void Start()
        {
            m_owner = GetComponentInParent<EnemyController>();
            foreach (var state in States)
            {
                state.Initialize(this);
            }

            CurrentState = States[0];
            CurrentState.EnterState();
        }

        public void ActivateBrain(bool isActive)
        {
            m_brainActive = isActive;
            if (m_brainActive)
            {
                CurrentState.EnterState();
            }
        }

        public void ResetBrain()
        {
            CurrentState = States[0];
            foreach (var state in States)
            {
                foreach (var transition in state.Transitions)
                {
                    transition.BrainDecision.OnReset();
                }
            }
        }
        
        private void Update()
        {
            if (!m_brainActive) return;

            if (CurrentState == null) return;
            
            CurrentState.DoActions();
            
            if (!m_brainActive) return;
            
            CurrentState.CheckTransitions();

            TimeInState += Time.deltaTime;
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
            foreach (var state in States)
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
    }
}
