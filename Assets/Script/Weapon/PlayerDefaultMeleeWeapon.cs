using System.Collections;
using SGGames.Script.Core;
using SGGames.Script.Entity;
using UnityEngine;

namespace SGGames.Script.Weapons
{
    public class PlayerDefaultMeleeWeapon : Weapon
    {
        [SerializeField] private int m_actionPointCost;
        [SerializeField] private Collider2D[] m_weaponColliders;

        private bool m_isAttacking;
        private PlayerController m_playerController;
        private IWeaponOwner m_owner;
        private const float k_DELAY_AFTER_ENABLE_WEAPON_COLLIDER = 0.25f;
        
        public override void InitializeWeapon(IWeaponOwner owner)
        {
            m_owner = owner;
            m_playerController = ((PlayerWeaponHandler)owner).GetComponent<PlayerController>();
            foreach (var weaponCollider in m_weaponColliders)
            {
                weaponCollider.enabled = false;
            }
        }

        public override void ChangeState(Global.WeaponState nextState)
        {
            
        }

        public override void UpdateAnimationOnAttack()
        {
            
        }

        public override void RotateWeapon(Vector3 aimdirection, float aimAngle)
        {
            transform.rotation = Quaternion.AngleAxis(aimAngle, Vector3.forward);
            base.RotateWeapon(aimdirection, aimAngle);
        }

        public override void Attack()
        {
            if (!m_playerController.ActionPoint.CanUsePoint(m_actionPointCost)) return;
            if (m_isAttacking) return;
            
            m_playerController.ActionPoint.UsePoints(m_actionPointCost);
            
            StartCoroutine(OnAttackProcess());
            
            base.Attack();
        }

        private IEnumerator OnAttackProcess()
        {
            m_isAttacking = true;
            
            foreach (var weaponCollider in m_weaponColliders)
            {
                weaponCollider.enabled = true;
            }
            yield return new WaitForSeconds(k_DELAY_AFTER_ENABLE_WEAPON_COLLIDER);
            foreach (var weaponCollider in m_weaponColliders)
            {
                weaponCollider.enabled = false;
            }
            
            if (!m_playerController.ActionPoint.StillHavePoints())
            {
                m_playerController.FinishedTurn();
            }

            m_isAttacking = false;
        }
    }
}

