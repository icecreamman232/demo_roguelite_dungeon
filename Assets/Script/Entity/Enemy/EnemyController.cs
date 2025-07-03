using System;
using SGGames.Script.Core;
using SGGames.Script.Dungeon;
using SGGames.Script.Entity;
using SGGames.Script.Events;
using SGGames.Script.HealthSystem;
using SGGames.Script.Managers;
using UnityEngine;

namespace SGGames.Scripts.Entity
{
    public class EnemyController : EntityBehavior
    {
        [SerializeField] private GameEvent m_gameEvent;
        [SerializeField] protected EnemyBrain m_currentBrain;
        
        private EnemyHealth m_health;

        public EnemyBrain CurrentBrain => m_currentBrain;

        private void Awake()
        {
            m_gameEvent.AddListener(OnReceiveGameEvent);
            m_health = GetComponent<EnemyHealth>();
            m_health.OnDeath += OnEnemyDeath;

            var room = GetComponentInParent<Room>();
            room.RegisterEnemyToRoom(this);
            m_health.OnDeath += room.OnEnemyDeath;

            var gameManager = ServiceLocator.GetService<GameManager>();
            gameManager.OnGamePauseCallback += OnGamePaused;
            gameManager.OnGameUnPauseCallback += OnGameResumed;
        }
        
        private void OnEnemyDeath()
        {
            m_health.OnDeath -= OnEnemyDeath;
            m_currentBrain.BrainActive = false;
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
    }
}

