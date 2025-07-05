using SGGames.Script.Core;
using SGGames.Script.Data;
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
        
        private BoxCollider2D m_boxCollider2D;
        private bool m_canMove;
        
        private void Start()
        {
            var inputManager = ServiceLocator.GetService<InputManager>();
            inputManager.OnMoveInputUpdate += UpdateMoveInput;
            m_moveSpeed = m_playerData.MoveSpeed;
            m_boxCollider2D = GetComponent<BoxCollider2D>();
            
            var gameManager = ServiceLocator.GetService<GameManager>();
            gameManager.OnGamePauseCallback += OnGamePaused;
            gameManager.OnGameUnPauseCallback += OnGameResumed;

            SetMovementType(Global.MovementType.Normal);
            m_canMove = true;
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
            var hit = Physics2D.BoxCast(transform.position, m_boxCollider2D.size, 0, m_movementDirection,m_raycastDistance,m_obstacleLayerMask);
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
        }
    }
}
