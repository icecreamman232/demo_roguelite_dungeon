using SGGames.Script.Data;
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

        public void StopMoving()
        {
            m_canMove = false;
            m_movementDirection = Vector2.zero;
        }
    }
}
