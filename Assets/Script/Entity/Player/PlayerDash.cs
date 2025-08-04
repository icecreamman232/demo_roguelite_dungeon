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
        [SerializeField] private WorldEvent m_worldEvent;
        [SerializeField] private AnimationCurve m_dashSpeedCurve;
        [SerializeField] private AfterImageFX m_afterImageFX;
        [SerializeField] private SpriteRenderer m_spriteRenderer;

        private PlayerController m_controller;
        private bool m_isDashing;
        private Vector3 m_startPosition;
        private Vector3 m_endPosition;
        private float m_traveledDistance;
        private float m_distanceToTarget;
        private Vector2 m_lastMoveInput;

        private IDashCommand[] m_startDashCommands;
        private IDashCommand[] m_endDashCommands;


        public Action OnDashHitObstacle;
        public Action OnDashFinished;
        
        private void Start()
        {
            m_controller = GetComponent<PlayerController>();
            
            var inputManager = ServiceLocator.GetService<InputManager>();
            inputManager.OnMoveInputUpdate += UpdateMoveInput;
            inputManager.OnPressDash += OnDashButtonPressed;

            m_lastMoveInput = Vector2.right;

            SetupCommands();
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

            if (m_controller.PlayerMovement.IsHitObstacle)
            {
                OnDashHitObstacle?.Invoke();
                m_worldEvent.Raise(Global.WorldEventType.OnPlayerDashCanceled, this.gameObject);
                EndDash();
                return;
            }
            
            var traveledTime = MathHelpers.Remap(m_traveledDistance,0,m_distanceToTarget,0,1);
            var speedMultiplier = m_dashSpeedCurve.Evaluate(traveledTime);
            transform.position = Vector3.MoveTowards(transform.position,m_endPosition, speedMultiplier * m_playerData.DashSpeed * Time.deltaTime);
            m_traveledDistance = Vector3.Distance(m_startPosition, transform.position);

            m_afterImageFX.DropImageFX(m_spriteRenderer.sprite, m_spriteRenderer.flipX);

            if (transform.position == m_endPosition)
            {
                OnDashFinished?.Invoke();
                m_worldEvent.Raise(Global.WorldEventType.OnPlayerStopDash, this.gameObject);
                EndDash();
            }
        }

        private void PrepareBeforeDash()
        {
            m_startPosition = transform.position;
            m_endPosition = m_startPosition + (Vector3)m_lastMoveInput * m_playerData.DashDistance;
            
            m_distanceToTarget = Vector3.Distance(m_startPosition, m_endPosition);
            m_traveledDistance = 0;
            
            foreach (var command in m_startDashCommands)
            {
                command.Execute();
            }
            
            m_worldEvent.Raise(Global.WorldEventType.OnPlayerStartDash, this.gameObject);
            
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
    }
}
