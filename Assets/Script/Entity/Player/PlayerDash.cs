using System;
using System.Collections.Generic;
using SGGames.Scripts.Core;
using SGGames.Scripts.Data;
using SGGames.Scripts.Events;
using SGGames.Scripts.Items;
using SGGames.Scripts.Modules;
using SGGames.Scripts.Managers;
using SGGames.Scripts.PathFindings;
using UnityEngine;

namespace SGGames.Scripts.Entities
{
    public class PlayerDash : EntityBehavior, IPlayerSpecialAbility
    {
        [SerializeField] private Global.PlayerDashState m_dashState;
        [SerializeField] private PlayerData m_playerData;
        [SerializeField] private WorldEvent m_worldEvent;
        [SerializeField] private AnimationCurve m_dashSpeedCurve;
        [SerializeField] private AfterImageFX m_afterImageFX;
        [SerializeField] private SpriteRenderer m_spriteRenderer;
        [Header("Events")]
        [SerializeField] private DisplayEffectTileEvent m_displayEffectTileEvent;
        [SerializeField] private PlayerUseActionPointEvent m_playerUseActionPointEvent;
        [SerializeField] private SwitchTurnEvent m_switchTurnEvent;
        [SerializeField] private HudButtonEvent m_hudButtonEvent;
        [SerializeField] private AbilityStateEvent m_abilityStateEvent;
        [SerializeField] private AbilityCooldownEvent m_abilityCooldownEvent;

        private int m_cooldownTimer;
        private PlayerController m_controller;
        private float m_lerpValue;
        private Vector3 m_startPosition;
        private Vector3 m_endPosition;
        private Vector3 m_dashDirection;
        private GridManager m_gridManager;
        
        private const float k_raycastDistance = 0.5f;
        
        private IDashCommand[] m_startDashCommands;
        private IDashCommand[] m_endDashCommands;
        
        public Action OnDashHitObstacle;
        public Action OnDashFinished;

        #region Unity Cycle Methods
        
        private void Awake()
        {
            InternalInitialize();
        }
        
        private void Start()
        {
            ExternalInitialize();
        }

        private void OnDestroy()
        {
            m_switchTurnEvent.RemoveListener(OnSwitchTurn);
            m_hudButtonEvent.RemoveListener(OnHudButtonEvent);
        }

        private void Update()
        {
            UpdateMovement();
        }
        
        #endregion
        
        #region Initialize
        
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
            inputManager.OnCancel += CancelPrepareDash;
            
            m_gridManager = ServiceLocator.GetService<GridManager>();
            m_switchTurnEvent.AddListener(OnSwitchTurn);
            m_hudButtonEvent.AddListener(OnHudButtonEvent);
            m_dashState = Global.PlayerDashState.Ready;
        }
        
        private void InternalInitialize()
        {
            m_dashDirection = Vector2.right;
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
        
        #endregion
        
        #region Dash speed modifier
        
        /// <summary>
        /// To be removed
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="percentageBonus"></param>
        [Obsolete]
        public void AddPercentageBonusSpeedFromItem(Guid guid, float percentageBonus)
        {
           
        }

        /// <summary>
        /// To be removed
        /// </summary>
        /// <param name="guid"></param>
        [Obsolete]
        public void RemovePercentageBonusSpeedFromItem(Guid guid)
        {
            
        }

        /// <summary>
        /// To be removed
        /// </summary>
        /// <param name="bonusSpeed"></param>
        [Obsolete]
        public void AddFlatBonusSpeedFromItem(float bonusSpeed)
        {
            
        }

        /// <summary>
        /// To be removed
        /// </summary>
        /// <param name="bonusSpeed"></param>
        [Obsolete]
        public void RemoveFlatBonusSpeedFromItem(float bonusSpeed)
        {
            
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
        
        #region Dash Methods

        public bool CanDash()
        {
            if (!m_controller.PlayerStamina.CanUseStamina(m_playerData.StaminaCostForDash))
            {
                Debug.Log("Dash:: Can't use stamina");
                return false;
            }

            if (!m_controller.ActionPoint.CanUsePoint(1))
            {
                Debug.Log("Dash:: Can't use AP");
                return false;
            }

            if (m_dashState != Global.PlayerDashState.Ready)
            {
                Debug.Log($"Dash:: Dash state not ready. Current state {m_dashState.ToString()}");
                return false;
            }
            
            return true;
        }

        public void ForceEndDash()
        {
            EndDash();
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
            if(m_dashState != Global.PlayerDashState.ShowRange) return;
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

        private void BlockComponentBeforeDash()
        {
            m_controller.PlayerMovement.SetPermission(false);
            m_controller.WeaponHandler.SetPermission(false);
        }

        private void UnlockComponentAfterDash()
        {
            m_controller.PlayerMovement.SetPermission(true);
            m_controller.WeaponHandler.SetPermission(true);
        }
        
        private void PrepareBeforeDash()
        {
            m_dashState = Global.PlayerDashState.Dashing;
            m_abilityStateEvent.Raise(new AbilityStateEventData
            {
                AbilityState = Global.AbilityState.Executing,
                abilityType = Global.AbilityType.Special
            });
            
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
            
        }

        /// <summary>
        /// Hide the range indicator and reset prepared parameters for dash
        /// </summary>
        private void CancelPrepareDash()
        {
            UnlockComponentAfterDash();
            m_abilityStateEvent.Raise(new AbilityStateEventData
            {
                AbilityState = Global.AbilityState.Ready,
                abilityType = Global.AbilityType.Special
            });
            m_dashState = Global.PlayerDashState.Ready;
            HideRangeIndicator(m_dashDirection, m_playerData.DashDistance);
        }
        
        private void UpdateMovement()
        {
            if (!m_isPermit) return;
            if(m_dashState != Global.PlayerDashState.Dashing) return;
            
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
            
            if (m_lerpValue >= 1 || Vector3.Distance(transform.position, m_endPosition) <= 0.1f)
            {
                transform.position = m_endPosition;
                OnDashFinished?.Invoke();
                m_worldEvent.Raise(Global.WorldEventType.OnPlayerStopDash, this.gameObject, null);
                EndDash();
            }
        }
        
        private void EndDash()
        {
            m_controller.PlayerMovement.ResetMovementParameters();
            if (!m_controller.ActionPoint.StillHavePoints())
            {
                m_controller.FinishedTurn();
            }
            foreach (var command in m_endDashCommands)
            {
                command.Execute();
            }
            UnlockComponentAfterDash();
            m_cooldownTimer = m_playerData.DashCooldown;
            
            m_abilityCooldownEvent.Raise(new AbilityCooldownEventData
            {
                AbilityType = Global.AbilityType.Special,
                Cooldown = m_cooldownTimer
            });
            
            m_abilityStateEvent.Raise(new AbilityStateEventData
            {
                AbilityState = Global.AbilityState.Cooldown,
                abilityType = Global.AbilityType.Special
            });
            m_dashState = Global.PlayerDashState.Cooldown;
        }

        private void CountCooldown()
        {
            if(m_dashState != Global.PlayerDashState.Cooldown) return;
            m_cooldownTimer--;
            if (m_cooldownTimer <= 0)
            {
                m_cooldownTimer = 0;
                m_abilityStateEvent.Raise(new AbilityStateEventData
                {
                    AbilityState = Global.AbilityState.Ready,
                    abilityType = Global.AbilityType.Special
                });
                m_dashState = Global.PlayerDashState.Ready;
            }
            
            m_abilityCooldownEvent.Raise(new AbilityCooldownEventData
            {
                AbilityType = Global.AbilityType.Special,
                Cooldown = m_cooldownTimer
            });
        }

        #endregion
        
        #region Callbacks
        
        private void OnAimingDataChanged(AimingData aimingData)
        {
            if (m_dashState != Global.PlayerDashState.ShowRange) return;
            m_dashDirection = aimingData.AimDirection;
            ShowRangeIndicator(aimingData.AimDirection, m_playerData.DashDistance);
        }
        
        private void OnDashButtonPressed()
        {
            if (!CanDash())
            {
                //Play cant dash animation
                m_controller.AnimationController.PlayCantMoveAnimation();
                return;
            }
            BlockComponentBeforeDash();
            m_abilityStateEvent.Raise(new AbilityStateEventData
            {
                AbilityState = Global.AbilityState.ShowRange,
                abilityType = Global.AbilityType.Special
            });
            m_dashState = Global.PlayerDashState.ShowRange;
        }
        
        private void OnExecuteButtonPressed()
        {
            if(m_dashState != Global.PlayerDashState.ShowRange) return;
            //Deduct stamina
            m_controller.PlayerStamina.UseStamina(m_playerData.StaminaCostForDash);
            //Deduct action point
            m_playerUseActionPointEvent.Raise(1);
            
            PrepareBeforeDash();
        }
        
        private void OnSwitchTurn(TurnBaseEventData turnBaseEventData)
        {
            if (turnBaseEventData.TurnBaseState == Global.TurnBaseState.PlayerTakeTurn)
            {
                CountCooldown();
            }
        }
        
        private void OnHudButtonEvent(HudButtonEventData hudButtonEventData)
        {
            switch (hudButtonEventData.HudButtonType)
            {
                case Global.HudButtonType.SpecialAbilityButton:
                    if (!CanDash())
                    {
                        //Play cant dash animation
                        m_controller.AnimationController.PlayCantMoveAnimation();
                        return;
                    }
                    BlockComponentBeforeDash();
                    m_abilityStateEvent.Raise(new AbilityStateEventData
                    {
                        AbilityState = Global.AbilityState.ShowRange,
                        abilityType = Global.AbilityType.Special
                    });
                    m_dashState = Global.PlayerDashState.ShowRange;
                    break;
                case Global.HudButtonType.ExecuteAbilityButton:
                    if(m_dashState != Global.PlayerDashState.ShowRange) return;
                    //Deduct stamina
                    m_controller.PlayerStamina.UseStamina(m_playerData.StaminaCostForDash);
                    //Deduct action point
                    m_playerUseActionPointEvent.Raise(1);
            
                    PrepareBeforeDash();
                    break;
                case Global.HudButtonType.CancelAbilityButton:
                    CancelPrepareDash();
                    break;
            }
        }

        
        #endregion
        
        #region Special Ability Methods
        
        public bool StartSpecial()
        {
            if (!CanDash())
            {
                return false;
            }
            OnDashButtonPressed();
            return true;
        }

        public void ExecuteSpecial()
        {
            OnExecuteButtonPressed();
        }

        public void CancelSpecial()
        {
            CancelPrepareDash();
        }
        
        #endregion

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + m_dashDirection * 1);
        }
        
    }
}
