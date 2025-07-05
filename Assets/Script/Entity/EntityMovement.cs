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
            
        }

        protected virtual void UpdateStopMovement()
        {
            
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
