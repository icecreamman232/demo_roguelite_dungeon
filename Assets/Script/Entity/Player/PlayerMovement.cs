using SGGames.Script.Core;
using SGGames.Script.Data;
using SGGames.Script.EditorExtensions;
using SGGames.Script.Events;
using SGGames.Script.Managers;
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
        private float m_flatSpeedBonus;
        
        public bool IsHitObstacle => CheckObstacle();
        
        private void Start()
        {
            var inputManager = ServiceLocator.GetService<InputManager>();
            inputManager.OnMoveInputUpdate += UpdateMoveInput;
            m_moveSpeed = m_playerData.MoveSpeed;
            m_gameEvent.AddListener(OnReceiveGameEvent);
            
            
            m_controller = GetComponent<PlayerController>();
            var gameManager = ServiceLocator.GetService<GameManager>();
            gameManager.OnGamePauseCallback += OnGamePaused;
            gameManager.OnGameUnPauseCallback += OnGameResumed;

            SetMovementType(Global.MovementType.Normal);
            m_canMove = true;
            
            ConsoleCheatManager.RegisterCommands(this);
        }

        private void OnReceiveGameEvent(Global.GameEventType eventType)
        {
            if (eventType == Global.GameEventType.SpawnPlayer)
            {
                m_movementDirection = Vector2.zero;
            }
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

        protected override void UpdateNormalMovement()
        {
            base.UpdateNormalMovement();
            FlipModel();
        }

        protected override void FlipModel()
        {
            m_controller.SpriteRenderer.flipX = m_controller.WeaponHandler.AimDirection.x < 0;
            base.FlipModel();
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
            m_movementDirection = moveInput;
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

        [CheatCode("test","Description Here")]
        public void TestCheatCode(int testValue)
        {
            Debug.Log($"Applied Cheat Code  {testValue}");
        }
    }
}
