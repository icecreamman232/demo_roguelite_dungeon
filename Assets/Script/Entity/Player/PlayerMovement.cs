using System;
using SGGames.Script.Core;
using SGGames.Script.Data;
using SGGames.Script.EditorExtensions;
using SGGames.Script.Events;
using SGGames.Script.Managers;
using SGGames.Script.Modules;
using UnityEngine;

namespace SGGames.Script.Entity
{
    public class PlayerMovement : EntityMovement
    {
        [Header("Player")] 
        [SerializeField] private float m_raycastDistance;
        [SerializeField] private LayerMask m_obstacleLayerMask;
        [SerializeField] private PlayerData m_playerData;
        [SerializeField] private GameEvent m_gameEvent;

        private PlayerController m_controller;
        private bool m_canMove;
        private PercentageStackController m_percentageStackController;
        private float m_flatSpeedBonus;

        public LayerMask ObstacleLayerMask => m_obstacleLayerMask;
        public bool IsHitObstacle => CheckObstacle();

        protected override void Awake()
        {
            InternalInitialize();
            base.Awake();
        }

        private void Start()
        {
            ExternalInitialize();
        }
        
        private void OnDisable()
        {
            var inputManager = ServiceLocator.GetService<InputManager>();
            if (inputManager != null)
            {
                inputManager.OnMoveInputUpdate -= UpdateMoveInput;
            }
            m_gameEvent.RemoveListener(OnReceiveGameEvent);
        }

        private void InternalInitialize()
        {
            m_controller = GetComponent<PlayerController>();
            SetMovementType(Global.MovementType.Normal);
            m_percentageStackController = new PercentageStackController();
            m_canMove = true;
        }

        private void ExternalInitialize()
        {
            var inputManager = ServiceLocator.GetService<InputManager>();
            inputManager.OnMoveInputUpdate += UpdateMoveInput;
            
            var gameManager = ServiceLocator.GetService<GameManager>();
            gameManager.OnGamePauseCallback += OnGamePaused;
            gameManager.OnGameUnPauseCallback += OnGameResumed;
            
            m_gameEvent.AddListener(OnReceiveGameEvent);
            ConsoleCheatManager.RegisterCommands(this);
        }
        
        public void PauseMovement()
        {
            m_canMove = false;    
        }

        public void ResumeMovement()
        {
            m_canMove = true;
        }

        public void AddFlatSpeedBonus(float bonus)
        {
            m_flatSpeedBonus += bonus;
        }
        public void RemoveFlatSpeedBonus(float bonus)
        {
            m_flatSpeedBonus -= bonus;
        }

        protected override void OnGamePaused()
        {
            m_canMove = false;
            base.OnGamePaused();
        }

        protected override void OnGameResumed()
        {
            m_canMove = true;
            base.OnGameResumed();
        }

        protected override void UpdateMovement()
        {
            if (!m_canMove) return;
            
            if (CheckObstacle())
            {
                m_movementDirection = Vector2.zero;
            }
            base.UpdateMovement();
        }

        private bool CheckObstacle()
        {
            var hit = Physics2D.BoxCast(transform.position, m_controller.PlayerCollider.size, 0, m_movementDirection,m_raycastDistance,m_obstacleLayerMask);
            if (hit.collider != null)
            {
                return true;
            }

            return false;
        }

        private void UpdateMoveInput(Vector2 moveInput)
        {
            if (m_currentMovementState != Global.MovementState.Ready) return;
            
            m_movementDirection = moveInput;
            if (m_movementDirection != Vector2.zero)
            {
                CalculateNextPosition();
                SetMovementState(Global.MovementState.Moving);
            }
        }

        private void CalculateNextPosition()
        {
            m_nextPosition = transform.position + (Vector3)m_movementDirection;
        }
        
        private void OnReceiveGameEvent(Global.GameEventType eventType)
        {
            if (eventType == Global.GameEventType.SpawnPlayer)
            {
                m_movementDirection = Vector2.zero;
            }
        }

        [CheatCode("test","Description Here")]
        public void TestCheatCode(int testValue)
        {
            Debug.Log($"Applied Cheat Code  {testValue}");
        }
    }
}
