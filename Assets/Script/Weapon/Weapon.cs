using SGGames.Script.Core;
using SGGames.Script.Managers;
using UnityEngine;

namespace SGGames.Script.Weapons
{
    public class Weapon : MonoBehaviour
    {
        private Animator m_animator;
        private readonly int ATTACK_LEFT_ANIM_TRIGGER = Animator.StringToHash("Trigger_Slash_Left");
        private readonly int ATTACK_RIGHT_ANIM_TRIGGER = Animator.StringToHash("Trigger_Slash_Right");
        
        private bool m_isAttackOnLeft;
        
        private void Start()
        {
            m_animator = GetComponent<Animator>();
            var inputManager = ServiceLocator.GetService<InputManager>();
            inputManager.OnPressAttack += OnAttackTriggered;
        }

        private void OnAttackTriggered()
        {
            
            m_animator.SetTrigger(m_isAttackOnLeft ? ATTACK_LEFT_ANIM_TRIGGER : ATTACK_RIGHT_ANIM_TRIGGER);
        }

        public void SetAttackOnLeft(bool isAttackOnLeft)
        {
            m_isAttackOnLeft = isAttackOnLeft;
        }
    }
}
