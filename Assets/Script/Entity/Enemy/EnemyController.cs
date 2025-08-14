using System;
using System.Collections.Generic;
using SGGames.Script.Core;
using SGGames.Script.Dungeon;
using SGGames.Script.Entity;
using SGGames.Script.Events;
using SGGames.Script.HealthSystem;
using SGGames.Script.Managers;
using SGGames.Script.Modules;
using SGGames.Scripts.AI;
using UnityEngine;

namespace SGGames.Scripts.Entity
{
    [Serializable]
    public class EnemyController : EntityBehavior, IRevivable , IEntityIdentifier
    {
        [SerializeField] protected SwitchTurnEvent m_switchTurnEvent;
        [SerializeField] protected GameEvent m_gameEvent;
        [SerializeField] protected SpriteRenderer m_spriteRenderer;
        [SerializeField] protected EnemyHealth m_enemyHealth;
        [SerializeField] protected EnemyMovement m_enemyMovement;
        [SerializeField] protected EnemyWeaponHandler m_enemyWeaponHandler;
        [SerializeField] protected EnemyAnimationController m_animationController;
        [SerializeField] protected AIBrain m_aiBrain;
        
        private List<IDeathCommand> m_deathCommands;
        private int m_orderIndex;
        public int OrderIndex => m_orderIndex;
        public AIBrain AIBrain => m_aiBrain;
        public EnemyMovement Movement => m_enemyMovement;
        public EnemyHealth Health => m_enemyHealth;
        public EnemyWeaponHandler WeaponHandler => m_enemyWeaponHandler;

        private void Awake()
        {
            m_gameEvent.AddListener(OnReceiveGameEvent);
            m_enemyHealth.OnDeath += OnEnemyDeath;
            
            
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

        private void Start()
        {
            m_enemyMovement.Initialize(this);
            m_enemyWeaponHandler.Initialize(this);
            m_aiBrain.Initialize(this);
            var turnBaseManager = ServiceLocator.GetService<TurnBaseManager>();
            turnBaseManager.RegisterEnemy(this);
        }

        private void OnDestroy()
        {
            var gameManager = ServiceLocator.GetService<GameManager>();
            gameManager.OnGamePauseCallback -= OnGamePaused;
            gameManager.OnGameUnPauseCallback -= OnGameResumed;
            m_gameEvent.RemoveListener(OnReceiveGameEvent);
        }

        public void SetOrderIndex(int orderIndex)
        {
            m_orderIndex = orderIndex;
        }

        public void FinishTurn()
        {
            m_switchTurnEvent.Raise(new TurnBaseEventData
            {
                TurnBaseState = Global.TurnBaseState.EnemyFinishedTurn,
                EntityIndex = m_orderIndex
            });
        }

        private void RegisterEnemyToRoom()
        {
            if (m_enemyHealth.CanRevive) return;
            
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
                if (!m_enemyHealth.CanRevive)
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
        }

        protected override void OnGamePaused()
        {
            //m_currentBrain.ActivateBrain(false);
            base.OnGamePaused();
        }

        protected override void OnGameResumed()
        {
            //m_currentBrain.ActivateBrain(true);
            base.OnGameResumed();
        }

        public Global.Direction DirectionToTarget()
        {
            var target = m_aiBrain.Target;
            var directionToTarget = (target.transform.position - transform.position).normalized;
            var directionEnumValue = Global.Direction.Left;
            if (directionToTarget == Vector3.left)
            {
                directionEnumValue = Global.Direction.Left;
            }
            else if (directionToTarget == Vector3.right)
            {
                directionEnumValue = Global.Direction.Right;
            }
            else if (directionToTarget == Vector3.up)
            {
                directionEnumValue = Global.Direction.Up;
            }
            else if (directionToTarget == Vector3.down)
            {
                directionEnumValue = Global.Direction.Down;
            }
            return directionEnumValue;
        }
        
        public void OnRevive()
        {
            
        }
        #region Facade Methods

        public void FlipSprite(Vector2 direction)
        {
            //m_animationController.FlipModel(direction.x < 0);
        }
        #endregion
        
        #region Cheat Code
        public void Kill()
        {
            m_enemyHealth.SelfKill();
        }
        #endregion

        public bool IsPlayer()
        {
            return false;
        }
    }
}

