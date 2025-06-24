using UnityEngine;

namespace SGGames.Script.Entity
{
    public class EntityMovement : EntityBehavior
    {
        [Header("Movement")]
        [SerializeField] protected float m_moveSpeed;
        [SerializeField] protected Vector2 m_movementDirection;
        
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
            transform.Translate((m_moveSpeed * Time.deltaTime) * m_movementDirection);
        }
    }
}
