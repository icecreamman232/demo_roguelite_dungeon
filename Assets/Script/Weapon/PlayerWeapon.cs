using System.Collections;
using SGGames.Script.Core;
using SGGames.Script.Managers;
using UnityEngine;

namespace SGGames.Script.Weapons
{
    public class PlayerWeapon : Weapon
    {
        [SerializeField] private AnimationClip m_attackAnimation;
        private Collider2D m_collider2D;
        private Animator m_animator;
        private readonly int ATTACK_LEFT_ANIM_TRIGGER = Animator.StringToHash("Trigger_Slash_Left");
        private readonly int ATTACK_RIGHT_ANIM_TRIGGER = Animator.StringToHash("Trigger_Slash_Right");

        private float m_attackAnimDuration;
        private bool m_isAttackOnLeft;
        private bool m_isAttacking;
        
        private void Start()
        {
            m_animator = GetComponent<Animator>();
            m_attackAnimDuration = m_attackAnimation.length;
            var inputManager = ServiceLocator.GetService<InputManager>();
            inputManager.OnPressAttack += OnAttackTriggered;
            m_collider2D = GetComponent<Collider2D>();
            m_collider2D.enabled = false;
        }

        private void OnAttackTriggered()
        {
            if (m_isAttacking) return;
            m_animator.SetTrigger(m_isAttackOnLeft ? ATTACK_LEFT_ANIM_TRIGGER : ATTACK_RIGHT_ANIM_TRIGGER);
            StartCoroutine(OnEnableWeaponCollider());
        }

        private IEnumerator OnEnableWeaponCollider()
        {
            m_collider2D.enabled = true;
            yield return new WaitForSeconds(m_attackAnimDuration);
            m_collider2D.enabled = false;
        }

        public void SetAttackOnLeft(bool isAttackOnLeft)
        {
            m_isAttackOnLeft = isAttackOnLeft;
        }
    }
}
