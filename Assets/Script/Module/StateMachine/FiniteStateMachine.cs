using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SGGames.Scripts.Modules
{
    public interface IState<T>
    {
        void Enter(T context);
        void Update(T context);
        void Exit(T context);
    }
    
    public class FiniteStateMachine<TContext, TStateEnum> where TStateEnum : System.Enum
    {
        private TContext m_context;
        private IState<TContext> m_currentState;
        private Dictionary<TStateEnum, IState<TContext>> m_cacheStateDictionary;
        
        public IState<TContext> CurrentState => m_currentState;

        public FiniteStateMachine(TContext context, (TStateEnum, IState<TContext>)[] states)
        {
            m_context = context;
            m_cacheStateDictionary = new Dictionary<TStateEnum, IState<TContext>>();
            foreach (var (state, stateInstance) in states)
            {
                m_cacheStateDictionary.Add(state, stateInstance);
            }
            
            // Set initial state to the first state if available
            if (m_cacheStateDictionary.Count > 0)
            {
                m_currentState = m_cacheStateDictionary.First().Value;
            }
        }

        public void SetState(TStateEnum newStateType)
        {
            if (!m_cacheStateDictionary.TryGetValue(newStateType, out var newState))
            {
                Debug.LogError($"State {newStateType} not found");
                return;
            }
            
            m_currentState.Exit(m_context);
            m_currentState = newState;
            m_currentState.Enter(m_context);
        }

        public IState<TContext> GetState(TStateEnum stateType)
        {
            if (m_cacheStateDictionary.TryGetValue(stateType, out var state))
            {
                return state;
            }
            Debug.LogError($"State {stateType} not found");
            
            //Fallback to the current state to avoid null error
            return m_currentState;
        }

        public bool HasState(TStateEnum stateType)
        {
            return m_cacheStateDictionary.ContainsKey(stateType);
        }
        
        public void Update()
        {
            m_currentState?.Update(m_context);
        }

        public void AddState(TStateEnum stateType, IState<TContext> state)
        {
            m_cacheStateDictionary.Add(stateType, state);
        }
        
        public void RemoveState(TStateEnum stateType)
        {
            m_cacheStateDictionary.Remove(stateType);
        }
    }
}
