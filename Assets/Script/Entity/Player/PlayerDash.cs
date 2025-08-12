using System;
using System.Collections;
using SGGames.Script.Core;
using SGGames.Script.Data;
using SGGames.Script.Items;
using SGGames.Script.Managers;
using SGGames.Script.Modules;
using UnityEngine;

namespace SGGames.Script.Entity
{
    public class PlayerDash : EntityBehavior
    {
        [SerializeField] private PlayerData m_playerData;
        [SerializeField] private float m_currentSpeed;
        [SerializeField] private WorldEvent m_worldEvent;
        [SerializeField] private AnimationCurve m_dashSpeedCurve;
        [SerializeField] private AfterImageFX m_afterImageFX;
        [SerializeField] private SpriteRenderer m_spriteRenderer;

        private PercentageStackController m_percentageStackController;
        private float m_flatSpeedBonus;
        
        private PlayerController m_controller;
        private bool m_isDashing;
        private Vector3 m_startPosition;
        private Vector3 m_endPosition;
        private float m_traveledDistance;
        private float m_distanceToTarget;
        private Vector2 m_lastMoveInput;

        private IDashCommand[] m_startDashCommands;
        private IDashCommand[] m_endDashCommands;
        
        public float CurrentSpeed => m_currentSpeed;
        
        public Action OnDashHitObstacle;
        public Action OnDashFinished;
        
        private void Start()
        {
            m_controller = GetComponent<PlayerController>();
            m_percentageStackController = new PercentageStackController();
            
            var inputManager = ServiceLocator.GetService<InputManager>();
            inputManager.OnMoveInputUpdate += UpdateMoveInput;
            inputManager.OnPressDash += OnDashButtonPressed;

            m_lastMoveInput = Vector2.right;

            m_currentSpeed = m_playerData.DashSpeed;
            
            SetupCommands();
        }
        
        #region Dash speed modifier
        public void AddPercentageBonusSpeedFromItem(Guid guid, float percentageBonus)
        {
            m_percentageStackController.AddPercentage(guid, percentageBonus);
            m_currentSpeed = m_percentageStackController.GetValueWithPercentageStack(m_playerData.DashSpeed);
            Debug.Log($"Apply dash speed modifier {percentageBonus} % - Current Spd: {m_currentSpeed}");
        }

        public void RemovePercentageBonusSpeedFromItem(Guid guid)
        {
            m_percentageStackController.RemovePercentage(guid);
            m_currentSpeed = m_percentageStackController.GetValueWithPercentageStack(m_playerData.DashSpeed);
            Debug.Log($"Remove dash speed modifier - Current Spd: {m_currentSpeed}");
        }

        public void AddFlatBonusSpeedFromItem(float bonusSpeed)
        {
            m_flatSpeedBonus += bonusSpeed;
        }

        public void RemoveFlatBonusSpeedFromItem(float bonusSpeed)
        {
            m_flatSpeedBonus -= bonusSpeed;
        }
        
        #endregion
        
        protected override void OnGamePaused()
        {
            SetPermission(false);
            base.OnGamePaused();
        }

        protected override void OnGameResumed()
        {
            SetPermission(true);
            base.OnGameResumed();
        }

        private void SetupCommands()
        {
            m_startDashCommands = new IDashCommand[]
            {
                new PauseMovementDashCommand(),
                new EnableInvincibleDashCommand(),
            };

            m_endDashCommands = new IDashCommand[]
            {
                new ResumeMovementDashCommand(),
                new DisableInvincibleDashCommand(),
            };

            foreach (var command in m_startDashCommands)
            {
                command.Initialize(this.gameObject);
            }
            
            foreach (var command in m_endDashCommands)
            {
                command.Initialize(this.gameObject);
            }
        }

        private void OnDashButtonPressed()
        {
            if (!m_controller.PlayerStamina.CanUseStamina(m_playerData.StaminaCostForDash)) return;

            m_controller.PlayerStamina.UseStamina(m_playerData.StaminaCostForDash);
            PrepareBeforeDash();
        }

        private void UpdateMoveInput(Vector2 lastMoveInput)
        {
            if (lastMoveInput == Vector2.zero) return;
            m_lastMoveInput = lastMoveInput;
        }

        private void Update()
        {
            if (!m_isPermit) return;
            if (!m_isDashing) return;

            // if (m_controller.PlayerMovement.IsHitObstacle)
            // {
            //     OnDashHitObstacle?.Invoke();
            //     m_worldEvent.Raise(Global.WorldEventType.OnPlayerDashCanceled, this.gameObject, null);
            //     EndDash();
            //     return;
            // }
            
            var traveledTime = MathHelpers.Remap(m_traveledDistance,0,m_distanceToTarget,0,1);
            var speedMultiplier = m_dashSpeedCurve.Evaluate(traveledTime);
            var finalDashSpeed = (speedMultiplier * m_currentSpeed);
            finalDashSpeed = Mathf.Clamp(finalDashSpeed,m_playerData.DashSpeed,m_playerData.MaxDashSpeed);
            transform.position = Vector3.MoveTowards(transform.position,m_endPosition, finalDashSpeed * Time.deltaTime);
            m_traveledDistance = Vector3.Distance(m_startPosition, transform.position);

            m_afterImageFX.DropImageFX(m_spriteRenderer.sprite, m_spriteRenderer.flipX);

            if (transform.position == m_endPosition)
            {
                OnDashFinished?.Invoke();
                m_worldEvent.Raise(Global.WorldEventType.OnPlayerStopDash, this.gameObject, null);
                EndDash();
            }
        }
        
        private Vector3 FindSafeDashEndPosition(Vector3 startPos, Vector3 desiredEndPos)
        {
            Vector3 direction = (desiredEndPos - startPos).normalized;
            float maxDistance = Vector3.Distance(startPos, desiredEndPos);
    
            var playerCollider = m_controller.PlayerCollider;
    
            // Step backwards from desired position to find safe spot
            for (float distance = maxDistance; distance > 0.1f; distance -= 0.1f)
            {
                Vector3 testPosition = startPos + direction * distance;
        
                // Check if this position would cause a collision
                var hit = Physics2D.OverlapBox(testPosition, playerCollider.size, 0, m_controller.PlayerMovement.ObstacleLayerMask);
        
                if (hit == null)
                {
                    return testPosition;
                }
            }
    
            return startPos;
        }


        private void PrepareBeforeDash()
        {
            m_startPosition = transform.position;
            var desiredEndPosition = m_startPosition + (Vector3)m_lastMoveInput * m_playerData.DashDistance;
            m_endPosition = FindSafeDashEndPosition(m_startPosition, desiredEndPosition);
            m_distanceToTarget = Vector3.Distance(m_startPosition, m_endPosition);
            m_traveledDistance = 0;
            
            foreach (var command in m_startDashCommands)
            {
                command.Execute();
            }
            
            m_worldEvent.Raise(Global.WorldEventType.OnPlayerStartDash, this.gameObject, null);
            
            m_isDashing = true;
        }

        private void EndDash()
        {
            m_isDashing = false;
            foreach (var command in m_endDashCommands)
            {
                command.Execute();
            }
            StartCoroutine(OnCoolDown());
        }

        private IEnumerator OnCoolDown()
        {
            yield return new WaitForSeconds(m_playerData.DashCooldown);
        }
    }
}
