using System;
using SGGames.Script.Core;
using SGGames.Script.Managers;
using SGGames.Script.Weapons;
using UnityEngine;

namespace SGGames.Script.Entity
{
    public class PlayerWeaponHandler : EntityBehavior, IWeaponOwner
    {
        [SerializeField] private Transform m_aimingCursor;
        [SerializeReference] private Weapon m_currWeapon;
        
        private PlayerController m_playerController;
        private Vector3 m_aimDirection;

        public Vector3 AimDirection => m_aimDirection;
        public PlayerController Controller => m_playerController;
        public Action OnAttack;
        
        private void Start()
        {
            var inputManager = ServiceLocator.GetService<InputManager>();
            inputManager.OnPressAttack += OnPressAttackButton;
            m_currWeapon.InitializeWeapon(this);
        }

        public void Initialize(PlayerController playerController)
        {
            m_playerController = playerController;
            m_playerController.AimingController.OnAimingDataChanged += OnAimingDataChanged;
        }

        private void OnAimingDataChanged(AimingData aimingData)
        {
            m_aimDirection = aimingData.AimDirection;
            RotateAimingCursor(aimingData.AimAngle);
            m_currWeapon.RotateWeapon(aimingData.AimDirection, aimingData.AimAngle);
        }

        private void OnPressAttackButton()
        {
            if (!m_isPermit) return;
            if (m_playerController.PlayerMovement.CurrentMovementState != Global.MovementState.Ready) return;
            m_currWeapon.Attack();
            OnAttack?.Invoke();
        }
     
        private void RotateAimingCursor(float aimAngle)
        {
            m_aimingCursor.rotation = Quaternion.AngleAxis(aimAngle, Vector3.forward);
        }

        public void ForceResetCombo()
        {
           // m_currWeapon.ForceResetCombo();
        }
    }
}