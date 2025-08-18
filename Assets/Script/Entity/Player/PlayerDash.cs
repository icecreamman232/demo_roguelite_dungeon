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
using UnityEditor.Experimental.GraphView;
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
        private float m_lerpValue;
        private bool m_isDashing;
        private Vector3 m_startPosition;
        private Vector3 m_endPosition;
        private Vector3 m_dashDirection;
        private bool m_allowShowRangeHUD;
        private GridManager m_gridManager;
        
        private const float k_raycastDistance = 0.5f;
        
        private IDashCommand[] m_startDashCommands;
        private IDashCommand[] m_endDashCommands;
        
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

        private bool CheckObstacleWithRaycast(Vector3 direction)
        {
            var hit = Physics2D.Raycast(transform.position, direction, k_raycastDistance, LayerManager.ObstacleMask | LayerManager.DoorMask);
            return hit.collider != null;
        }
        
        private bool CheckCollisionAtThisPosition(Vector3 position, LayerMask layerMask)
        {
            return Physics2D.OverlapBox(position, m_controller.PlayerCollider.size, 0, layerMask) != null;
        }

        private void ShowRangeIndicator(Vector3 direction, int range)
        {
            if (!m_allowShowRangeHUD) return;
            var positionList = new List<Vector3>();
            for (int i = 0; i <= range; i++)
            {
                var snapPos = m_gridManager.GetSnapPosition(transform.position + direction * i);
                var isObstacle = CheckCollisionAtThisPosition(snapPos, LayerManager.ObstacleMask | LayerManager.DoorMask);
                if (!isObstacle)
                {
                    positionList.Add(snapPos);
                }
                else
                {
                    break;
                }
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
            for (int i = 0; i <= range; i++)
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
            
            if (CheckObstacleWithRaycast(m_dashDirection))
            {
                OnDashHitObstacle?.Invoke();
                m_worldEvent.Raise(Global.WorldEventType.OnPlayerDashCanceled, this.gameObject, null);
                EndDash();
                return;
            }

            m_lerpValue += Time.deltaTime * m_playerData.DashSpeed;
            transform.position = Vector3.Lerp(m_startPosition, m_endPosition, m_lerpValue);

            m_afterImageFX.DropImageFX(m_spriteRenderer.sprite, m_spriteRenderer.flipX);
            
            if (transform.position == m_endPosition)
            {
                OnDashFinished?.Invoke();
                m_worldEvent.Raise(Global.WorldEventType.OnPlayerStopDash, this.gameObject, null);
                EndDash();
            }
        }
        
        
        private void PrepareBeforeDash()
        {
            m_allowShowRangeHUD = false;
            HideRangeIndicator(m_dashDirection, m_playerData.DashDistance);
            
            m_startPosition = transform.position;
            m_endPosition = m_startPosition + m_dashDirection * m_playerData.DashDistance;

            //Check if dash ends at the enemy's position, then we will extend dash distance 1 unit behind the enemy
            if (CheckCollisionAtThisPosition(m_endPosition, LayerManager.EnemyMask))
            {
                m_endPosition = m_startPosition + m_dashDirection * (m_playerData.DashDistance + 1);
            }

            m_lerpValue = 0;
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

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + m_dashDirection * 1);
        }
    }
}
