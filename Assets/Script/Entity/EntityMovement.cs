using SGGames.Script.Core;
using UnityEngine;

namespace SGGames.Script.Entity
{
    public class EntityMovement : EntityBehavior
    {
        [Header("Movement")]
        [SerializeField] protected Global.MovementType m_movementType;
        [SerializeField] protected float m_moveSpeed;
        [SerializeField] protected Vector2 m_movementDirection;

        protected float m_knockBackSpeed;
        protected Global.MovementType m_prevMovementType;
        protected Vector2 m_prevMovementDirection;
        protected float m_knockBackEndTime;
        protected delegate void UpdateMovementDelegate();
        
        protected UpdateMovementDelegate m_movementDelegateMethod;
        
        protected virtual void Update()
        {
            if (!m_isPermit) return;
            
            UpdateInput();
            UpdateMovement();
        }

        /// <summary>
        /// Base method to update all input involved momevent
        /// </summary>
        protected virtual void UpdateInput()
        {
            
        }
        
        /// <summary>
        /// Base method to update movement. Warning, all update for input should be processed in UpdateInput
        /// </summary>
        protected virtual void UpdateMovement()
        {
            m_movementDelegateMethod?.Invoke();
        }

        protected virtual void UpdateNormalMovement()
        {
            transform.Translate((m_moveSpeed * Time.deltaTime) * m_movementDirection);
        }

        protected virtual void UpdateKnockBackMovement()
        {
            if (Time.time > m_knockBackEndTime
                || m_knockBackSpeed <= 0)
            {
                //End knockback update
                m_movementType = m_prevMovementType;
                m_movementDirection = m_prevMovementDirection;
                SetMovementType(m_movementType);
            }
            
            m_knockBackSpeed -= Time.deltaTime;
            //m_knockBackSpeed = Mathf.Clamp(m_knockBackSpeed, 0, m_moveSpeed);
            transform.Translate(m_movementDirection * (m_knockBackSpeed * Time.deltaTime));
        }

        protected virtual void UpdateStopMovement()
        {
            
        }

        protected virtual void FlipModel()
        {
            
        }

        public void ApplyKnockBack(Vector2 direction, float force, float duration)
        {
            if (m_movementType == Global.MovementType.KnockBack) return;

            //Save previous state value
            m_prevMovementDirection = m_movementDirection;
            m_prevMovementType = m_movementType;

            m_movementDirection = direction;
            m_knockBackSpeed = force;
            m_knockBackEndTime = duration + Time.time;
            
            SetMovementType(Global.MovementType.KnockBack);
        }

        public void SetMovementType(Global.MovementType movementType)
        {
            switch (movementType)
            {
                case Global.MovementType.Normal:
                    m_movementType = Global.MovementType.Normal;
                    m_movementDelegateMethod = UpdateNormalMovement;
                    break;
                case Global.MovementType.KnockBack:
                    m_movementType = Global.MovementType.KnockBack;
                    m_movementDelegateMethod = UpdateKnockBackMovement;
                    break;
                case Global.MovementType.Stop:
                    m_movementType = Global.MovementType.Stop;
                    m_movementDelegateMethod = UpdateStopMovement;
                    break;
            }
        }
    }
}
