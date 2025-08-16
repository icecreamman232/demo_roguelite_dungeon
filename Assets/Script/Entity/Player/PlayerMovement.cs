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
        [SerializeField] private Global.MovementDirectionType m_movementDirectionType;
        [SerializeField] private float m_raycastDistance;
        [SerializeField] private LayerMask m_obstacleLayerMask;
        [SerializeField] private PlayerData m_playerData;
        [Header("Events")]
        [SerializeField] private GameEvent m_gameEvent;
        [SerializeField] private PlayerUseActionPointEvent m_playerUseActionPointEvent;

        private PlayerController m_controller;
        private bool m_canMove;
        
        public Global.MovementState CurrentMovementState => m_currentMovementState;

        public LayerMask ObstacleLayerMask => m_obstacleLayerMask;
        public bool IsHitObstacle(Vector3 direction) => CheckObstacle(direction);

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
            m_gameEvent.RemoveListener(OnReceiveGameEvent);
        }

        private void InternalInitialize()
        {
            m_controller = GetComponent<PlayerController>();
            SetMovementState(Global.MovementState.Ready);
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
            if (!m_canMove)
            {
                // If movement is disabled while moving, reset to Ready state
                if (m_currentMovementState == Global.MovementState.Moving)
                {
                    SetMovementState(Global.MovementState.Ready);
                }
                return;
            }
            
            base.UpdateMovement();
        }

        protected override void OnFinishMovement()
        {
            base.OnFinishMovement();
            if (!m_controller.ActionPoint.StillHavePoints())
            {
                m_controller.FinishedTurn();
            }
        }

        private bool CheckObstacle(Vector3 direction)
        {
            var hit = Physics2D.BoxCast(transform.position, m_controller.PlayerCollider.size, 0, direction,m_raycastDistance,m_obstacleLayerMask);
            return hit.collider != null;
        }

        private bool CanMove()
        {
            if (m_currentMovementState != Global.MovementState.Ready) return false;
            if (!m_controller.ActionPoint.CanUsePoint(1)) return false;
            
            return true;
        }

        private void UpdateMoveInput(Vector2 moveInput)
        {
            if (m_movementDirectionType == Global.MovementDirectionType.FourDirections)
            {
                //Limit to 4 directions movement
                if (Mathf.Abs(moveInput.x) > Mathf.Abs(moveInput.y))
                {
                    moveInput = new Vector2(moveInput.x, 0);
                }
                else if (Mathf.Abs(moveInput.y) > 0)
                {
                    moveInput = new Vector2(0, moveInput.y);
                }
            }
            
            
            if (!CanMove() || CheckObstacle(moveInput))
            {
                if (moveInput != Vector2.zero)
                {
                    m_controller.AnimationController.PlayCantMoveAnimation();
                }
                return;
            }
            
            m_movementDirection= moveInput;
            if (m_movementDirection != Vector2.zero)
            {
                CalculateNextPosition();
                m_playerUseActionPointEvent.Raise(1);
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
