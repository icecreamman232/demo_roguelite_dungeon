using SGGames.Script.Core;
using SGGames.Script.Events;
using SGGames.Scripts.Entity;
using UnityEngine;

namespace SGGames.Scripts.AI
{
    public class AIBrain : MonoBehaviour
    {
        [SerializeField] private SwitchTurnEvent m_switchTurnEvent;
        [SerializeField] private bool m_isActivated;
        [SerializeField] private Transform m_target;
        [SerializeField] private AIState[] m_states;
        [SerializeField] private AIState m_currentState;
        
        private EnemyController m_owner;
        public EnemyController Owner => m_owner;
        
        public Transform Target
        {
            get => m_target;
            set => m_target = value;
        }

        private void Awake()
        {
            m_switchTurnEvent.AddListener(OnSwitchTurnEvent);
        }

        private void OnDisable()
        {
            m_switchTurnEvent.RemoveListener(OnSwitchTurnEvent);
        }
        
        private void Update()
        {
            if (!m_isActivated) return;
            
            if (m_currentState != null)
            {
                m_currentState.UpdateActionProgress();
            }
        }

        public void CompleteTurn()
        {
            m_currentState = null;
            m_owner.FinishTurn();
            m_isActivated = false;
        }
        
        public void Initialize(EnemyController controller)
        {
            m_owner = controller;
            m_currentState = null;
            
            for (int i = 0; i < m_states.Length; i++)
            {
                m_states[i].Initialize(this);
            }
        }
       
        private void ExecuteTurn()
        {
            Debug.Log("Executing Turn===========================================");
            m_isActivated = true;
            
            //Start at first state down to last state
            m_currentState = m_states[0];
            
            for (int i = 0; i < m_states.Length; i++)
            {
                if (m_states[i].CheckDecision())
                {
                    m_currentState = m_states[i];
                    m_states[i].DoActions();
                    Debug.Log($"Do actions at state {m_states[i].StateName}");
                    if (m_states[i].ConsumeTurn)
                    {
                        Debug.Log($"Break at state {m_states[i].StateName}");
                        break;
                    }
                }
            }
        }

        private void ResetStates()
        {
            foreach (var state in m_states)
            {
                state.ResetActions();
            }
        }
        
        private void OnSwitchTurnEvent(TurnBaseEventData turnBaseEventData)
        {
            if (turnBaseEventData.TurnBaseState == Global.TurnBaseState.EnemyTakeTurn
                && m_owner.OrderIndex == turnBaseEventData.EntityIndex)
            {
                ResetStates();
                ExecuteTurn();
            }
        }
        
        
        #region Editor
        public AIAction[] GetAttachedActions()
        {
            var actions = GetComponentsInChildren<AIAction>();
            return actions;
        }

        public AIDecision[] GetAttachedDecisions()
        {
            var decisions = GetComponentsInChildren<AIDecision>();
            return decisions;
        }
        #endregion
    }
}
