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
        
        private SpriteRenderer m_spriteRenderer;
        private EnemyHealth m_health;
        private List<IDeathCommand> m_deathCommands;

        public EnemyBrain CurrentBrain => m_currentBrain;

        private void Awake()
        {
            m_currentBrain = m_defaultBrain;
            m_currentBrain.Initialize(this);
            
            m_gameEvent.AddListener(OnReceiveGameEvent);
            m_health = GetComponent<EnemyHealth>();
            m_health.OnDeath += OnEnemyDeath;

            m_spriteRenderer = GetComponentInChildren<SpriteRenderer>();

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
        
        private void OnDestroy()
        {
            var gameManager = ServiceLocator.GetService<GameManager>();
            gameManager.OnGamePauseCallback -= OnGamePaused;
            gameManager.OnGameUnPauseCallback -= OnGameResumed;
            
            m_gameEvent.RemoveListener(OnReceiveGameEvent);
        }

        private void RegisterEnemyToRoom()
        {
            if (m_health.CanRevive) return;
            
            var room = GetComponentInParent<Room>();
            room.RegisterEnemyToRoom(this);
        }
        
        private void OnEnemyDeath()
        {
            foreach (var command in m_deathCommands)
            {
                command.Execute();
            }

            var room = GetComponentInParent<Room>();
            if (room.HasThisEnemyRegistered(this))
            {
                room.OnEnemyDeath();
            }
            else
            {
                if (!m_health.CanRevive)
                {
                    room.RegisterEnemyToRoom(this);
                }
            }
        }
        
        private void OnReceiveGameEvent(Global.GameEventType eventType)
        {
            if (eventType == Global.GameEventType.RoomCreated)
            {
                RegisterEnemyToRoom();
            }
            else if(eventType ==  Global.GameEventType.GameStarted)
            {
                m_currentBrain.ActivateBrain(true);
            }
        }

        protected override void OnGamePaused()
        {
            m_currentBrain.ActivateBrain(false);
            base.OnGamePaused();
        }

        protected override void OnGameResumed()
        {
            m_currentBrain.ActivateBrain(true);
            base.OnGameResumed();
        }

        public void SetActiveBrain(EnemyBrain newBrain)
        {
            m_currentBrain = newBrain;
            m_currentBrain.Initialize(this);
        }
        
        public void OnRevive()
        {
            m_currentBrain = m_defaultBrain;
            
            m_currentBrain.gameObject.SetActive(true);
            m_currentBrain.ResetBrain();
            m_currentBrain.ActivateBrain(true);
        }
        #region Facade Methods

        public void FlipSprite(Vector2 direction)
        {
            if (m_spriteRenderer == null)
            {
                Debug.LogError("Sprite Renderer is null");
                return;
            }
            
            m_spriteRenderer.flipX = direction.x < 0;
        }
        #endregion
        
        #region Cheat Code
        public void Kill()
        {
            m_health.SelfKill();
        }
        #endregion
        
    }
}

