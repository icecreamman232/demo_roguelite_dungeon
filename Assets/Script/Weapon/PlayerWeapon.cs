using SGGames.Script.Core;
using SGGames.Script.Entity;
using SGGames.Script.Managers;
using UnityEngine;

namespace SGGames.Script.Weapons
{
    public class PlayerWeapon : Weapon
    {
        [SerializeField] private AnimationClip m_attackAnimation;
        private Animator m_animator;
        private readonly int ATTACK_LEFT_ANIM_TRIGGER = Animator.StringToHash("Trigger_Slash_Left");
        private readonly int ATTACK_RIGHT_ANIM_TRIGGER = Animator.StringToHash("Trigger_Slash_Right");

        private float m_attackAnimDuration;
        private bool m_isAttackOnLeft;
        private bool m_isAttacking;
        private PlayerWeaponHandler m_playerWeaponHandler;
        private ProjectileBuilder m_projectileBuilder;
        
        private void Start()
        {
            m_animator = GetComponent<Animator>();
            m_attackAnimDuration = m_attackAnimation.length;
        }

        public override void Initialize(GameObject owner)
        {
            m_playerWeaponHandler = owner.GetComponent<PlayerWeaponHandler>();
            m_projectileBuilder = new ProjectileBuilder();
            base.Initialize(owner);
        }

        protected override void SpawnProjectile()
        {
            var projectileRot = Quaternion.AngleAxis(Mathf.Atan2(m_playerWeaponHandler.AimDirection.y, m_playerWeaponHandler.AimDirection.x) * Mathf.Rad2Deg, Vector3.forward);
            var projectileGO = m_projectilePooler.GetPooledGameObject();
            var projectile = projectileGO.GetComponent<Projectile>();
            projectile.Spawn(m_projectileBuilder
                .SetOwner(m_owner)
                .SetDirection(m_playerWeaponHandler.AimDirection)
                .SetPosition(transform.position)
                .SetRotation(projectileRot));
        }

        protected override void UpdateAnimatorOnAttack()
        {
            m_animator.SetTrigger(m_isAttackOnLeft ? ATTACK_LEFT_ANIM_TRIGGER : ATTACK_RIGHT_ANIM_TRIGGER);
            base.UpdateAnimatorOnAttack();
        }

        public void SetAttackOnLeft(bool isAttackOnLeft)
        {
            m_isAttackOnLeft = isAttackOnLeft;
        }
    }
}
