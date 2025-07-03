using SGGames.Script.Core;
using SGGames.Script.Data;
using SGGames.Script.Managers;
using UnityEngine;

namespace SGGames.Script.Entity
{
    public class EnemyMovement : EntityMovement
    {
        [Header("Enemy")]
        [SerializeField] private EnemyData m_enemyData;

        private bool m_canMove;

        public Vector2 MoveDirection => m_movementDirection;
        
        private void Awake()
        {
            m_moveSpeed = m_enemyData.MoveSpeed;
            m_canMove = true;
            var gameManager = ServiceLocator.GetService<GameManager>();
            gameManager.OnGamePauseCallback += OnGamePaused;
            gameManager.OnGameUnPauseCallback += OnGameResumed;
        }

        protected override void OnGamePaused()
        {
            PauseMoving();
            base.OnGamePaused();
        }

        protected override void OnGameResumed()
        {
            ResumeMoving();
            base.OnGameResumed();
        }

        public void SetDirection(Vector2 dir)
        {
            m_movementDirection = dir;
        }

        public void StartMoving()
        {
            m_canMove = true;
        }

        public void PauseMoving()
        {
            m_canMove = false;
        }

        public void ResumeMoving()
        {
            m_canMove = m_movementDirection == Vector2.zero;
        }

        public void StopMoving()
        {
            m_canMove = false;
            m_movementDirection = Vector2.zero;
        }

        private void OnDestroy()
        {
            var gameManager = ServiceLocator.GetService<GameManager>();
            gameManager.OnGamePauseCallback -= OnGamePaused;
            gameManager.OnGameUnPauseCallback -= OnGameResumed;
        }
    }
}
