using SGGames.Script.Core;
using SGGames.Script.Managers;
using SGGames.Script.Weapons;
using UnityEngine;

namespace SGGames.Script.Entity
{
    public class PlayerWeaponHandler : EntityBehavior
    {
        [SerializeField] private Transform m_weaponAttachment;
        [SerializeField] private PlayerWeapon m_currWeapon;
        
        private Vector3 m_aimDirection;
        private float m_aimAngle;
        private bool m_isAimAtLeftSide;
        
        private void Start()
        {
            var inputManager = ServiceLocator.GetService<InputManager>();
            inputManager.WorldMousePositionUpdate += OnWorldMousePositionChanged;
        }

        private void OnWorldMousePositionChanged(Vector3 mouseWorldPosition)
        {
            m_aimDirection = (mouseWorldPosition - transform.position).normalized;
            m_aimAngle = Mathf.Atan2(m_aimDirection.y, m_aimDirection.x) * Mathf.Rad2Deg;
            m_weaponAttachment.rotation = Quaternion.AngleAxis(m_aimAngle, Vector3.forward);

            m_currWeapon.transform.localScale = Vector3.one;
            m_isAimAtLeftSide = false;
            
            if ((m_aimAngle <= -90) || (m_aimAngle >= 90))
            {
                m_currWeapon.transform.localScale = new Vector3(1, -1, 1);
                m_isAimAtLeftSide = true;
            }
            
            m_currWeapon.SetAttackOnLeft(m_isAimAtLeftSide);
        }
    }
}
