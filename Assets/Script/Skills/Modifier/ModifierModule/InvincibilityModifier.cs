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
            m_playerController.PlayerHealth.SetInvincibleByItem(true);
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
            m_playerController.PlayerHealth.SetInvincibleByItem(false);
            m_playerController = null;
            Debug.Log($"Modifier::Remove Invincibility Modifier");
        }
    }
}
