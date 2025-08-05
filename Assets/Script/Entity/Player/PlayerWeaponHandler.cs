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
            m_aimDirection = (mouseWorldPosition - transform.position).normalized;
            m_aimAngle = Mathf.Atan2(m_aimDirection.y, m_aimDirection.x) * Mathf.Rad2Deg - 90f;
            m_weaponAttachment.rotation = Quaternion.AngleAxis(m_aimAngle, Vector3.forward);
            
            m_isAimAtLeftSide = m_aimAngle is <= -90 or >= 90;

            //m_currWeapon.SetAttackOnLeft(m_isAimAtLeftSide);
        }
    }
}
