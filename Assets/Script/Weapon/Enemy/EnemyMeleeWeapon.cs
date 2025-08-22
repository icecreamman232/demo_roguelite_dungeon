using System.Collections;
using SGGames.Scripts.Weapons;
using UnityEngine;

namespace SGGames.Scripts.Entities
{
    public class EnemyMeleeWeapon : Weapon
    {
        [SerializeField] private EnemyAnimationController m_animationController;
        [SerializeField] private Collider2D[] m_weaponColliders;
        
        private bool m_isAttacking;
        private EnemyWeaponHandler m_owner;
        private const float k_DELAY_AFTER_ENABLE_WEAPON_COLLIDER = 0.25f;
        
        public override void InitializeWeapon(IWeaponOwner owner)
        {
            m_owner = (EnemyWeaponHandler)owner;
            foreach (var weaponCollider in m_weaponColliders)
            {
                weaponCollider.enabled = false;
            }
            base.InitializeWeapon(owner);
        }

        protected override void UpdateAnimationOnAttack()
        {
            m_animationController.PlayBodySlamAnimation(m_owner.Controller.DirectionToTarget());
            base.UpdateAnimationOnAttack();
        }

        public override bool IsAttacking()
        {
            return m_isAttacking;
        }

        public override void Attack()
        {
            if (m_isAttacking) return;
            StartCoroutine(OnAttackProcess());
            base.Attack();
        }

        private IEnumerator OnAttackProcess()
        {
            EnableAttack();
            yield return new WaitForSeconds(k_DELAY_AFTER_ENABLE_WEAPON_COLLIDER);
            DisableAttack();
        }

        private void DisableAttack()
        {
            foreach (var weaponCollider in m_weaponColliders)
            {
                weaponCollider.enabled = false;
            }
            m_isAttacking = false;
        }

        private void EnableAttack()
        {
            m_isAttacking = true;
            foreach (var weaponCollider in m_weaponColliders)
            {
                weaponCollider.enabled = true;
            }
            UpdateAnimationOnAttack();
        }
    }
}
