using SGGames.Script.Core;
using SGGames.Script.Data;
using SGGames.Script.Entity;
using SGGames.Script.Events;
using UnityEngine;


namespace SGGames.Script.Entities
{
    public class PlayerActionPoint : EntityBehavior
    {
        [SerializeField] private PlayerData m_data;
        [Header("Events")]
        [SerializeField] private PlayerUseActionPointEvent m_playerUseActionPointEvent;
        [SerializeField] private UpdateActionPointUIEvent m_updateActionPointUIEvent;
        [SerializeField] private SwitchTurnEvent m_switchTurnEvent;
        [Header("Action Points")]
        [SerializeField] private int m_maxActionPoint;
        [SerializeField] private int m_currentActionPoint;

        public bool CanUsePoint(int amount) => m_currentActionPoint >= amount;
        public bool StillHavePoints() => m_currentActionPoint > 0;
        
        private void Awake()
        {
            InternalInitialize();
        }

        private void Start()
        {
            ExternalInitialize();
        }

        private void OnDestroy()
        {
            m_playerUseActionPointEvent.RemoveListener(UsePoints);
            m_switchTurnEvent.RemoveListener(OnSwitchTurnEvent);
        }

        private void InternalInitialize()
        {
            m_maxActionPoint = m_data.ActionPoints;
            m_currentActionPoint = m_maxActionPoint;
            UpdateActionPointUI();
        }

        private void ExternalInitialize()
        {
            m_playerUseActionPointEvent.AddListener(UsePoints);
            m_switchTurnEvent.AddListener(OnSwitchTurnEvent);
        }

        private void OnSwitchTurnEvent(TurnBaseEventData turnBaseEventData)
        {
            if (turnBaseEventData.TurnBaseState == Global.TurnBaseState.PlayerFinishedTurn)
            {
                ResetPoints();
            }
        }

        private void UpdateActionPointUI()
        {
            m_updateActionPointUIEvent.Raise(new ActionPointUIData
            {
                CurrentActionPoint = m_currentActionPoint,
                MaxActionPoint = m_maxActionPoint
            });
        }

        private void UsePoints(int points)
        {
            if (points > m_currentActionPoint) return;
            m_currentActionPoint -= points;
            UpdateActionPointUI();
        }

        private void ResetPoints()
        {
            m_currentActionPoint = m_maxActionPoint;
            UpdateActionPointUI();
        }
    }
}
