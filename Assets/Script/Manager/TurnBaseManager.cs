using System;
using System.Collections.Generic;
using System.Linq;
using SGGames.Script.Core;
using SGGames.Script.Events;
using SGGames.Scripts.Entity;
using UnityEngine;

namespace SGGames.Script.Managers
{
    [Serializable]
    public class EnemyTurnBaseStatus
    {
        public EnemyController RefController;
        public bool HasTakenTurn;
        public int OrderIndex;
    }
    
    public class TurnBaseManager : MonoBehaviour, IGameService
    {
        /// <summary>
        /// Indicate which entity is in this turn
        /// </summary>
        [SerializeField] private Global.TurnBaseState m_turnBaseState;
        [SerializeField] private SwitchTurnEvent m_switchTurnEvent;
        [SerializeField] private List<EnemyTurnBaseStatus> m_enemyTurnBaseStatusList;

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
            m_enemyTurnBaseStatusList.Clear();
            m_switchTurnEvent.RemoveListener(OnSwitchTurn);
        }

        public void RegisterEnemy(EnemyController controller)
        {
            m_enemyTurnBaseStatusList.Add(new EnemyTurnBaseStatus
            {
                RefController = controller,
                HasTakenTurn = false,
                OrderIndex = m_currentEnemyIndex,
            });
            controller.SetOrderIndex(m_currentEnemyIndex);
            m_currentEnemyIndex++;
        }

        private void InitializeInternal()
        {
            m_enemyTurnBaseStatusList = new List<EnemyTurnBaseStatus>();
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
                EntityIndex = GetNextReadyEnemyOrderIndex(),
            });
            
            m_turnBaseState = Global.TurnBaseState.EnemyTakeTurn;
        }

        private int GetNextReadyEnemyOrderIndex()
        {
            foreach (var enemyRef in m_enemyTurnBaseStatusList)
            {
                if(enemyRef.HasTakenTurn) continue;
                return enemyRef.OrderIndex;
            }
            return -1;
        }

        private bool HasAllEnemiesFinishedTurn()
        {
            bool hasAllFinished = true;
            foreach (var enemyRef in m_enemyTurnBaseStatusList)
            {
                if (!enemyRef.HasTakenTurn)
                {
                    hasAllFinished = false;
                }
            }
            return hasAllFinished;
        }

        private void ResetEnemyTurnBaseStatus()
        {
            foreach (var enemyRef in m_enemyTurnBaseStatusList)
            {
                enemyRef.HasTakenTurn = false;
            }
        }

        private void OnSwitchTurn(TurnBaseEventData turnBaseEventData)
        {
            switch (turnBaseEventData.TurnBaseState)
            {
                case Global.TurnBaseState.EnemyFinishedTurn:
                    Debug.Log("Finished Turn in Turn Base Manager");
                    m_enemyTurnBaseStatusList.First(enemy=>enemy.OrderIndex == turnBaseEventData.EntityIndex).HasTakenTurn = true;
                    if (HasAllEnemiesFinishedTurn())
                    {
                        m_currentEnemyIndex = 0;
                        ResetEnemyTurnBaseStatus();
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
