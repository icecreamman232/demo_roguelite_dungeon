using System;
using System.Collections.Generic;
using SGGames.Script.Core;
using SGGames.Script.Dungeon;
using SGGames.Script.Entity;
using SGGames.Script.Events;
using SGGames.Script.HealthSystem;
using SGGames.Script.Managers;
using SGGames.Script.Modules;
using UnityEngine;

namespace SGGames.Scripts.Entity
{
    public class EnemyController : EntityBehavior, IRevivable
    {
        [SerializeField] protected GameEvent m_gameEvent;
        [SerializeField] protected EnemyBrain m_defaultBrain;
        [SerializeField] protected EnemyBrain m_currentBrain;
        
        protected EnemyHealth m_health;
        private List<IDeathCommand> m_deathCommands;

        public EnemyBrain CurrentBrain => m_currentBrain;

        private void Awake()
        {
            m_currentBrain = m_defaultBrain;
            
            m_gameEvent.AddListener(OnReceiveGameEvent);
            m_health = GetComponent<EnemyHealth>();
            m_health.OnDeath += OnEnemyDeath;

            var room = GetComponentInParent<Room>();
            room.RegisterEnemyToRoom(this);
            m_health.OnDeath += room.OnEnemyDeath;

            var gameManager = ServiceLocator.GetService<GameManager>();
            gameManager.OnGamePauseCallback += OnGamePaused;
            gameManager.OnGameUnPauseCallback += OnGameResumed;
            
            m_deathCommands = new List<IDeathCommand>
            {
                new DisableMovementDeathCommand(),
                new DisableBrainDeathCommand()
            };

            foreach (var command in m_deathCommands)
            {
                command.Initialize(this);
            }
        }
        
        private void OnEnemyDeath()
        {
            foreach (var command in m_deathCommands)
            {
                command.Execute();
            }
        }

        protected override void OnGamePaused()
        {
            m_currentBrain.BrainActive = false;
            base.OnGamePaused();
        }

        protected override void OnGameResumed()
        {
            m_currentBrain.BrainActive = true;
            base.OnGameResumed();
        }

        public void SetActiveBrain(EnemyBrain newBrain)
        {
            m_currentBrain = newBrain;
        }
        
        private void OnReceiveGameEvent(Global.GameEventType eventType)
        {
            if(eventType ==  Global.GameEventType.GameStarted)
            {
                m_currentBrain.BrainActive = true;
            }
        }

        private void OnDestroy()
        {
            var gameManager = ServiceLocator.GetService<GameManager>();
            gameManager.OnGamePauseCallback -= OnGamePaused;
            gameManager.OnGameUnPauseCallback -= OnGameResumed;
        }

        public void OnRevive()
        {
            Debug.Log("EnemyController::OnRevive");
            m_currentBrain = m_defaultBrain;
            
            m_currentBrain.gameObject.SetActive(true);
            m_currentBrain.ResetBrain();
            m_currentBrain.BrainActive = true;
        }
    }
}

