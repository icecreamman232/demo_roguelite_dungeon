using System;
using SGGames.Script.Core;
using SGGames.Script.Data;
using SGGames.Script.Managers;
using UnityEngine;

namespace SGGames.Script.Entity
{
    public class EnemyMovement : EntityMovement
    {
        [Header("Enemy")] 
        [SerializeField] private Global.MovementBehaviorType m_movementBehaviorType;
        [SerializeField] private EnemyData m_enemyData;
        [SerializeField] private LayerMask m_obstacleLayerMask;
        private static float S_RAYCAST_DISTANCE = 0.15f;

        private BoxCollider2D m_collider2D;
        private RaycastHit2D m_collisionHit;
        private SpriteRenderer m_spriteRenderer;
        
        /// <summary>
        /// Target object for enemy either following or move towards
        /// </summary>
        private Transform m_target;

        public Action<bool> FlippingModelAction;
        public Vector2 MoveDirection => m_movementDirection;
        
        private void Awake()
        {
            m_moveSpeed = m_enemyData.MoveSpeed;
            var gameManager = ServiceLocator.GetService<GameManager>();
            gameManager.OnGamePauseCallback += OnGamePaused;
            gameManager.OnGameUnPauseCallback += OnGameResumed;

            m_collider2D = GetComponent<BoxCollider2D>();
            m_spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            SetMovementType(Global.MovementType.Normal);
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

        protected virtual bool IsCollideWithObstacle()
        {
            m_collisionHit = Physics2D.BoxCast(transform.position,m_collider2D.size, 0,m_movementDirection,S_RAYCAST_DISTANCE,m_obstacleLayerMask);    
            return m_collisionHit.collider != null;
        }

        protected override void UpdateMovement()
        {
            if (IsCollideWithObstacle())
            {
                StopMoving();
            }
            base.UpdateMovement();
        }

        protected override void UpdateNormalMovement()
        {
            if (m_movementBehaviorType == Global.MovementBehaviorType.FollowingTarget)
            {
                transform.position = Vector3.MoveTowards(transform.position, m_target.position, m_moveSpeed * Time.deltaTime);
            }
            else
            {
                base.UpdateNormalMovement();
            }
            
            FlipModel();
        }

        protected override void FlipModel()
        {
            
            if (m_movementBehaviorType == Global.MovementBehaviorType.FollowingTarget)
            {
                var direction = (m_target.position - transform.position).normalized;
                //m_spriteRenderer.flipX =  direction.x < 0;
                FlippingModelAction?.Invoke(direction.x < 0);
            }
            else
            {
                //m_spriteRenderer.flipX = m_movementDirection.x < 0;
                FlippingModelAction?.Invoke(m_movementDirection.x < 0);
            }
            
            base.FlipModel();
        }

        public void SetDirection(Vector2 dir)
        {
            m_movementDirection = dir;
        }

        public void StartMoving()
        {
            SetMovementType(Global.MovementType.Normal);
        }

        public void PauseMoving()
        {
            SetMovementType(Global.MovementType.Stop);
        }

        public void ResumeMoving()
        {
            SetMovementType(Global.MovementType.Normal);
        }

        public void StopMoving()
        {
            m_movementDirection = Vector2.zero;
            SetMovementType(Global.MovementType.Stop);
            SetMovementBehaviorType(Global.MovementBehaviorType.Normal);
        }

        public void SetFollowingTarget(Transform followingTarget)
        {
            m_target = followingTarget;
            //TODO:Handle the case where enemy is taking knockback then they have to finish the knockback before following target
            SetMovementType(Global.MovementType.Normal);
            SetMovementBehaviorType(Global.MovementBehaviorType.FollowingTarget);
        }

        public void SetMovementBehaviorType(Global.MovementBehaviorType movementBehaviorType)
        {
            m_movementBehaviorType = movementBehaviorType;
        }

        private void OnDestroy()
        {
            var gameManager = ServiceLocator.GetService<GameManager>();
            gameManager.OnGamePauseCallback -= OnGamePaused;
            gameManager.OnGameUnPauseCallback -= OnGameResumed;
        }
    }
}
