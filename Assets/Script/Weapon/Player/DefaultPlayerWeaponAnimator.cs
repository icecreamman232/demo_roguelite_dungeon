using UnityEngine;

namespace SGGames.Script.Weapons
{
    public class DefaultPlayerWeaponAnimator : IWeaponAnimator
    {
        private Animator m_animator;
        private bool m_isAttackOnLeft;
        
        private readonly int ATTACK_LEFT_ANIM_TRIGGER = Animator.StringToHash("Trigger_Slash_Left");
        private readonly int ATTACK_RIGHT_ANIM_TRIGGER = Animator.StringToHash("Trigger_Slash_Right");
        
        public void Initialize(Animator animator)
        {
            m_animator = animator;
        }

        public void UpdateAnimation()
        {
            m_animator.SetTrigger(m_isAttackOnLeft ? ATTACK_LEFT_ANIM_TRIGGER : ATTACK_RIGHT_ANIM_TRIGGER);
        }

        public void SetAttackDirection(bool isOnLeft)
        {
            m_isAttackOnLeft = isOnLeft;
        }
    }
}
