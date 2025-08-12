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
        
        private Vector3 m_aimDirection;
        private float m_aimAngle;
        
        public Vector3 AimDirection => m_aimDirection;

        public Action OnAttack;
        
        private void Start()
        {
            var inputManager = ServiceLocator.GetService<InputManager>();
            inputManager.WorldMousePositionUpdate += OnWorldMousePositionChanged;
            inputManager.OnPressAttack += OnPressAttackButton;
            m_currWeapon.InitializeWeapon(this);
        }

        private void OnPressAttackButton()
        {
            m_currWeapon.Attack();
            OnAttack?.Invoke();
        }

        private void OnWorldMousePositionChanged(Vector3 mouseWorldPosition)
        {
            CalculateAiming(mouseWorldPosition);
            RotateAimingCursor();
            m_currWeapon.RotateWeapon(m_aimDirection, m_aimAngle);
        }

        private void CalculateAiming(Vector3 mouseWorldPosition)
        {
            m_aimDirection = (mouseWorldPosition - transform.position).normalized;
            m_aimAngle = Mathf.Atan2(m_aimDirection.y, m_aimDirection.x) * Mathf.Rad2Deg - 90f;
            
            // Snap to 4 directions (90-degree increments)
            float snappedAngle = Mathf.Round(m_aimAngle / 90f) * 90f;
            
            // Update aim direction to match the snapped angle
            float radians = (snappedAngle + 90f) * Mathf.Deg2Rad;
            
            m_aimDirection = new Vector3(Mathf.Cos(radians), Mathf.Sin(radians), 0).normalized;
            m_aimAngle = snappedAngle;
        }
        
        private void RotateAimingCursor()
        {
            m_aimingCursor.rotation = Quaternion.AngleAxis(m_aimAngle, Vector3.forward);
        }

        public void ForceResetCombo()
        {
           // m_currWeapon.ForceResetCombo();
        }
    }
}