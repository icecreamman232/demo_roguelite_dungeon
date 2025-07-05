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
        
        private void Start()
        {
            m_animator = GetComponent<Animator>();
            m_attackAnimDuration = m_attackAnimation.length;
            var inputManager = ServiceLocator.GetService<InputManager>();
            inputManager.OnPressAttack += OnAttack;
        }

        public override void Initialize(GameObject owner)
        {
            m_playerWeaponHandler = owner.GetComponent<PlayerWeaponHandler>();
            base.Initialize(owner);
        }

        protected override void SpawnProjectile()
        {
            var projectileGO = m_projectilePooler.GetPooledGameObject();
            var projectile = projectileGO.GetComponent<Projectile>();
            projectile.Spawn(new ProjectileBuilder()
                .SetOwner(m_owner)
                .SetDirection(m_playerWeaponHandler.AimDirection)
                .SetPosition(transform.position));
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
