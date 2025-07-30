using System;
using SGGames.Script.Entities;
using SGGames.Script.HealthSystem;
using SGGames.Script.StaminaSystem;
using UnityEngine;

namespace SGGames.Script.Entity
{
    /// <summary>
    /// Central component for other components to access other components of player
    /// </summary>
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private BoxCollider2D m_collider;
        [SerializeField] private SpriteRenderer m_spriteRenderer;
        [SerializeField] private PlayerWeaponHandler m_weaponHandler;
        [SerializeField] private PlayerHealth m_playerHealth;
        [SerializeField] private PlayerStamina m_playerStamina;
        [SerializeField] private PlayerMovement m_playerMovement;
        [SerializeField] private PlayerDash m_playerDash;
        [SerializeField] private PlayerAnimationController m_animationController;
        [SerializeField] private PlayerResistanceController m_resistanceController;
        
        public BoxCollider2D PlayerCollider => m_collider;
        public GameObject Model => m_spriteRenderer.gameObject;
        public SpriteRenderer SpriteRenderer => m_spriteRenderer;
        public PlayerWeaponHandler WeaponHandler => m_weaponHandler;
        public PlayerHealth PlayerHealth => m_playerHealth;
        public PlayerStamina PlayerStamina => m_playerStamina;
        public PlayerMovement PlayerMovement => m_playerMovement;
        public PlayerDash PlayerDash => m_playerDash;
        public PlayerAnimationController AnimationController => m_animationController;
        public PlayerResistanceController ResistanceController => m_resistanceController;

        private void Start()
        {
            m_playerMovement.FlippingModelAction = m_animationController.FlipModel;
            m_playerHealth.Initialize(this, m_resistanceController);
            m_resistanceController.Initialize();
        }
    }
}
