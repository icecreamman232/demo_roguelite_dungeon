using System.Collections.Generic;
using SGGames.Script.Core;
using SGGames.Script.Events;
using SGGames.Scripts.Entity;
using UnityEngine;

namespace SGGames.Script.Managers
{
    public class TurnBaseManager : MonoBehaviour, IGameService
    {
        /// <summary>
        /// Indicate which entity is in this turn
        /// </summary>
        [SerializeField] private Global.TurnBaseState m_turnBaseState;
        [SerializeField] private SwitchTurnEvent m_switchTurnEvent;

        private List<EnemyController> m_availableEnemies;
        private int m_totalEnemy;
        private int m_currentEnemyIndex;
        
        private void Awake()
        {
            InitializeInternal();
        }

        private void Start()
        {
            InitializeExternal();
        }

        private void OnDestroy()
        {
            m_switchTurnEvent.RemoveListener(OnSwitchTurn);
        }

        public void RegisterEnemy(EnemyController controller)
        {
            m_availableEnemies.Add(controller);
            controller.SetOrderIndex(m_totalEnemy);
            m_totalEnemy++;
        }

        public void UnregisterEnemy(EnemyController controller)
        {
            m_availableEnemies.Remove(controller);
            m_totalEnemy--;
        }

        private void InitializeInternal()
        {
            m_availableEnemies = new List<EnemyController>();
            m_turnBaseState = Global.TurnBaseState.PlayerTakeTurn;
            m_switchTurnEvent.AddListener(OnSwitchTurn);
        }

        private void InitializeExternal()
        {
            ServiceLocator.RegisterService<TurnBaseManager>(this);  
        }

        private void SwitchToPlayerTurn()
        {
            m_switchTurnEvent.Raise(new TurnBaseEventData
            {
                TurnBaseState = Global.TurnBaseState.PlayerTakeTurn,
                EntityIndex = 0
            });
            
            m_turnBaseState = Global.TurnBaseState.PlayerTakeTurn;
        }

        private void SwitchToEnemyTurn()
        {
            m_switchTurnEvent.Raise(new TurnBaseEventData
            {
                TurnBaseState = Global.TurnBaseState.EnemyTakeTurn,
                EntityIndex = m_currentEnemyIndex,
            });
            m_currentEnemyIndex++;
            
            m_turnBaseState = Global.TurnBaseState.EnemyTakeTurn;
        }

        private void OnSwitchTurn(TurnBaseEventData turnBaseEventData)
        {
            Debug.Log($"TURN::{turnBaseEventData.TurnBaseState}");
            switch (turnBaseEventData.TurnBaseState)
            {
                case Global.TurnBaseState.EnemyFinishedTurn:
                    if (turnBaseEventData.EntityIndex >= m_totalEnemy - 1)
                    {
                        m_currentEnemyIndex = 0;
                        SwitchToPlayerTurn();
                    }
                    else
                    {
                        SwitchToEnemyTurn();
                    }
                    break;
                case Global.TurnBaseState.PlayerFinishedTurn:
                    SwitchToEnemyTurn();
                    break;
            }
        }
    }
}
