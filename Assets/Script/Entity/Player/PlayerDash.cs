using System;
using System.Collections;
using System.Collections.Generic;
using SGGames.Script.Core;
using SGGames.Script.Data;
using SGGames.Script.Events;
using SGGames.Script.Items;
using SGGames.Script.Managers;
using SGGames.Script.Modules;
using SGGames.Script.PathFindings;
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
        [Header("Events")]
        [SerializeField] private DisplayEffectTileEvent m_displayEffectTileEvent;
        [SerializeField] private PlayerUseActionPointEvent m_playerUseActionPointEvent;

        private PercentageStackController m_percentageStackController;
        private float m_flatSpeedBonus;
        
        private PlayerController m_controller;
        private bool m_isDashing;
        private Vector3 m_startPosition;
        private Vector3 m_endPosition;
        private float m_traveledDistance;
        private float m_distanceToTarget;
        private Vector3 m_dashDirection;
        private bool m_allowShowRangeHUD;
        private GridManager m_gridManager;
        
        
        private IDashCommand[] m_startDashCommands;
        private IDashCommand[] m_endDashCommands;
        
        public float CurrentSpeed => m_currentSpeed;
        
        public Action OnDashHitObstacle;
        public Action OnDashFinished;

        private void Awake()
        {
            InternalInitialize();
        }

        private void Start()
        {
            ExternalInitialize();
        }

        public void Initialize(PlayerController controller)
        {
            m_controller = controller;
            m_controller.AimingController.OnAimingDataChanged += OnAimingDataChanged;
        }
        
        private void ExternalInitialize()
        {
            var inputManager = ServiceLocator.GetService<InputManager>();
            inputManager.OnPressSpecialAbility += OnDashButtonPressed;
            inputManager.OnPressExecute += OnExecuteButtonPressed;
            
            m_gridManager = ServiceLocator.GetService<GridManager>();
        }

        private void InternalInitialize()
        {
            m_percentageStackController = new PercentageStackController();
            m_dashDirection = Vector2.right;
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

        private void OnExecuteButtonPressed()
        {
            //Deduct stamina
            m_controller.PlayerStamina.UseStamina(m_playerData.StaminaCostForDash);
            //Deduct action point
            m_playerUseActionPointEvent.Raise(1);
            
            PrepareBeforeDash();
        }

        private bool CanDash()
        {
            if(!m_controller.PlayerStamina.CanUseStamina(m_playerData.StaminaCostForDash)) return false;
            if (!m_controller.ActionPoint.CanUsePoint(1)) return false;
            return true;
        }
        
        private void OnDashButtonPressed()
        {
            if (!CanDash())
            {
                //Play cant dash animation
                m_controller.AnimationController.PlayCantMoveAnimation();
            }
            
            m_allowShowRangeHUD = true;
        }

        private void ShowRangeIndicator(Vector3 direction, int range)
        {
            if (!m_allowShowRangeHUD) return;
            var positionList = new List<Vector3>();
            //Start at i = 1 to avoid the player position. Therefore, the range will be (range + 1)
            for (int i = 1; i < (range + 1); i++)
            {
                positionList.Add(transform.position + direction * i);
            }
            m_displayEffectTileEvent.Raise(new EffectTileEventData
            {
                Position = positionList,
                EffectTileType = Global.EffectTileType.Indicator
            });
        }

        private void HideRangeIndicator(Vector3 direction, int range)
        {
            var positionList = new List<Vector3>();
            for (int i = 0; i < range; i++)
            {
                positionList.Add(transform.position + direction * i);
            }
            m_displayEffectTileEvent.Raise(new EffectTileEventData
            {
                Position = positionList,
                EffectTileType = Global.EffectTileType.None
            });
        }

        private void Update()
        {
            if (!m_isPermit) return;
            if (!m_isDashing) return;
            
            if (m_controller.PlayerMovement.CheckObstacleWithRaycast(m_dashDirection, 0.5f))
            {
                transform.position = m_gridManager.GetSnapPosition(transform.position);
                OnDashHitObstacle?.Invoke();
                m_worldEvent.Raise(Global.WorldEventType.OnPlayerDashCanceled, this.gameObject, null);
                EndDash();
                return;
            }
            
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
            m_allowShowRangeHUD = false;
            HideRangeIndicator(m_dashDirection, m_playerData.DashDistance);
            
            m_startPosition = transform.position;
            var desiredEndPosition = m_startPosition + m_dashDirection * m_playerData.DashDistance;
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
            m_controller.PlayerMovement.ResetMovementParameters();
            if (!m_controller.ActionPoint.StillHavePoints())
            {
                m_controller.FinishedTurn();
            }
            
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
        
        private void OnAimingDataChanged(AimingData aimingData)
        {
            if (!m_allowShowRangeHUD) return;
            m_dashDirection = aimingData.AimDirection;
            ShowRangeIndicator(aimingData.AimDirection, m_playerData.DashDistance);
        }
        
        [ContextMenu("Show Range")]
        private void Test()
        {
            transform.position = m_gridManager.GetSnapPosition(transform.position);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + m_dashDirection * 1);
        }
    }
}
