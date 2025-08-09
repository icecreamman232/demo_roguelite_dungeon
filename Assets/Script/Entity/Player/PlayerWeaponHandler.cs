using System;
using SGGames.Script.Core;
using SGGames.Script.Managers;
using SGGames.Script.Weapons;
using UnityEngine;

namespace SGGames.Script.Entity
{
    public class PlayerWeaponHandler : EntityBehavior, IWeaponOwner
    {
        [SerializeField] private Transform m_weaponAttachment;
        [SerializeField] private PlayerDefaultRangedWeapon m_currWeapon;
        
        private Vector3 m_aimDirection;
        private float m_aimAngle;
        private bool m_isAimAtLeftSide;
        
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
            Vector3 rawDirection = (mouseWorldPosition - transform.position).normalized;
    
            // Calculate the angle in degrees
            float rawAngle = Mathf.Atan2(rawDirection.y, rawDirection.x) * Mathf.Rad2Deg;
    
            // Snap to 8 directions (45-degree increments)
            float snappedAngle = Mathf.Round(rawAngle / 45f) * 45f;
    
            // Convert back to direction vector
            float angleInRadians = snappedAngle * Mathf.Deg2Rad;
            m_aimDirection = new Vector3(Mathf.Cos(angleInRadians), Mathf.Sin(angleInRadians), 0f);
    
            // Apply the -90 degree offset for weapon rotation
            m_aimAngle = snappedAngle - 90f;
            
            //Rotate using raw angle for smooth visual
            m_weaponAttachment.rotation = Quaternion.AngleAxis(rawAngle - 90f, Vector3.forward);
    
            m_isAimAtLeftSide = m_aimAngle is <= -90 or >= 90;
            //m_currWeapon.SetAttackOnLeft(m_isAimAtLeftSide);

        }

        public void ForceResetCombo()
        {
            m_currWeapon.ForceResetCombo();
        }
    }
}
