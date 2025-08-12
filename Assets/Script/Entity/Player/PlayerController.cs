using System;
using SGGames.Script.Core;
using SGGames.Script.Entities;
using SGGames.Script.Events;
using SGGames.Script.HealthSystem;
using SGGames.Script.StaminaSystem;
using UnityEngine;

namespace SGGames.Script.Entity
{
    /// <summary>
    /// Central component for other components to access other components of player
    /// </summary>
    [SelectionBase]
    public class PlayerController : MonoBehaviour, IEntityIdentifier
    {
        [SerializeField] private BoxCollider2D m_collider;
        [SerializeField] private SpriteRenderer m_spriteRenderer;
        [SerializeField] private PlayerWeaponHandler m_weaponHandler;
        [SerializeField] private PlayerHealth m_playerHealth;
        [SerializeField] private PlayerActionPoint m_actionPoint;
        [SerializeField] private PlayerStamina m_playerStamina;
        [SerializeField] private PlayerMovement m_playerMovement;
        [SerializeField] private PlayerDash m_playerDash;
        [SerializeField] private PlayerAnimationController m_animationController;
        [SerializeField] private PlayerResistanceController m_resistanceController;
        [SerializeField] private PlayerModifierController m_modifierController;
        [SerializeField] private PlayerDodge m_dodge;
        [SerializeField] private SwitchTurnEvent m_switchTurnEvent;

        public BoxCollider2D PlayerCollider => m_collider;
        public GameObject Model => m_spriteRenderer.gameObject;
        public SpriteRenderer SpriteRenderer => m_spriteRenderer;
        public PlayerWeaponHandler WeaponHandler => m_weaponHandler;
        public PlayerHealth Health => m_playerHealth;
        public PlayerActionPoint ActionPoint => m_actionPoint;
        public PlayerStamina PlayerStamina => m_playerStamina;
        public PlayerMovement PlayerMovement => m_playerMovement;
        public PlayerDash PlayerDash => m_playerDash;
        public PlayerAnimationController AnimationController => m_animationController;
        public PlayerResistanceController ResistanceController => m_resistanceController;
        public PlayerModifierController ModifierController => m_modifierController;
        public PlayerDodge PlayerDodge => m_dodge;

        private void Start()
        {
            m_switchTurnEvent.AddListener(OnSwitchTurn);
            m_playerHealth.Initialize(this, m_weaponHandler.ForceResetCombo);
            m_resistanceController.Initialize();
            ModifierController.Initialize(this);
        }

        private void OnSwitchTurn(TurnBaseEventData turnBaseEventData)
        {
            if (turnBaseEventData.TurnBaseState == Global.TurnBaseState.PlayerTakeTurn)
            {
                m_playerMovement.SetPermission(true);
            }
        }

        public bool IsPlayer()
        {
            return true;
        }

        public void FinishedTurn()
        {
            m_playerMovement.SetPermission(false);
            m_switchTurnEvent.Raise(new TurnBaseEventData
            {
                TurnBaseState = Global.TurnBaseState.PlayerFinishedTurn,
                EntityIndex = 0
            });
        }

        public void ForceResetCombo()
        {
            m_weaponHandler.ForceResetCombo();
        }
    }
}
