using System;
using SGGames.Script.Core;
using SGGames.Script.Modules;
using UnityEngine;

namespace SGGames.Script.Entity
{
    public class EntityMovement : EntityBehavior
    {
        [Header("Movement")] 
        [SerializeField] protected Global.MovementState m_currentMovementState;
        [SerializeField] protected Vector2 m_movementDirection;

        private TimerClock m_delayAfterMovingTimer;
        protected Vector3 m_currentPosition;
        protected Vector3 m_nextPosition;
        protected float m_lerpValue;
        private Action m_updateMovementAction;
        
        private const float k_MovementSpeed = 5;
        private const float k_DelayAfterMoving = 0.1f;

        protected virtual void Awake()
        {
            m_delayAfterMovingTimer = new TimerClock();
            m_currentPosition = transform.position;
            m_nextPosition = transform.position;
            m_movementDirection = Vector2.zero;
        }
        
        protected virtual void Update()
        {
            if (!m_isPermit) return;
            
            m_updateMovementAction?.Invoke();
        }
        
        protected virtual void OnFinishMovement()
        {
            SetMovementState(Global.MovementState.Ready);
        }
        
        /// <summary>
        /// Base method to update movement. Warning, all update for input should be processed in UpdateInput
        /// </summary>
        protected virtual void UpdateMovement()
        {
            m_lerpValue += Time.deltaTime * k_MovementSpeed;
            transform.position = Vector3.Lerp(m_currentPosition, m_nextPosition, m_lerpValue);
            if (m_lerpValue >= 1)
            {
                m_lerpValue = 0;
                m_currentPosition = m_nextPosition;
                SetMovementState(Global.MovementState.DelayAfterMoving);
            }
        }
        
        protected void SetMovementState(Global.MovementState movementState)
        {
            switch (movementState)
            {
                case Global.MovementState.Ready:
                    m_currentMovementState = Global.MovementState.Ready;
                    m_updateMovementAction = null;
                    break;
                case Global.MovementState.Moving:
                    m_currentMovementState = Global.MovementState.Moving;
                    m_updateMovementAction = UpdateMovement;
                    break;
                case Global.MovementState.DelayAfterMoving:
                    m_currentMovementState = Global.MovementState.DelayAfterMoving;
                    m_delayAfterMovingTimer.Start(k_DelayAfterMoving);
                    m_updateMovementAction = UpdateDelayAfterMoving;
                    break;
                case Global.MovementState.Finish:
                    m_updateMovementAction = null;
                    OnFinishMovement();
                    break;
            }
        }
        
        private void UpdateDelayAfterMoving()
        {
            if (m_delayAfterMovingTimer.IsRunning)
            {
                m_delayAfterMovingTimer.Update(Time.deltaTime);
            }
            else
            {
                SetMovementState(Global.MovementState.Finish);
            }
        }
    }
}
