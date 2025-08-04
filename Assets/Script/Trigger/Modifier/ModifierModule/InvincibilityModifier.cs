using System;
using SGGames.Script.Entity;
using UnityEngine;

namespace SGGames.Script.Skills
{
    [Serializable]
    public class InvincibilityModifier : Modifier
    {
        private float m_duration;
        private float m_timeElapsed;
        
        public InvincibilityModifier(PlayerController controller, float duration = 0) : base(controller)
        {
            m_duration = duration;
        }

        public override void Apply()
        {
            m_timeElapsed = m_duration;
            if (m_entity.IsPlayer())
            {
                ((PlayerController) m_entity).PlayerHealth.SetInvincibleByItem(true);
            }
            Debug.Log($"Modifier::Apply Invincibility Modifier");
        }

        public override void Update()
        {
            if (m_timeElapsed < 0)
            {
                m_shouldBeRemoved = true;
                return;
            }

            m_timeElapsed -= Time.deltaTime;
            base.Update();
        }

        public override void Remove()
        {
            if (m_entity.IsPlayer())
            {
                ((PlayerController) m_entity).PlayerHealth.SetInvincibleByItem(false);
            }
            Debug.Log($"Modifier::Remove Invincibility Modifier");
        }
    }
}
