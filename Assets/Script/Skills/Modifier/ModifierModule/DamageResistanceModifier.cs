using System;
using SGGames.Script.Entity;
using UnityEngine;

namespace SGGames.Script.Skills
{
    [Serializable]
    public class DamageResistanceModifier : Modifier
    {
        private float m_addingDamageResistance;
        private float m_duration;
        private float m_timeElapsed; 
        
        public DamageResistanceModifier(PlayerController controller, float addingDamageResistance, float duration)
            : base(controller)
        {
            m_addingDamageResistance = addingDamageResistance;
            m_duration = duration;
        }

        public override void Apply()
        {
            m_playerController.ResistanceController.AddDamageResistance(m_addingDamageResistance);
            if (m_duration > 0)
            {
                m_timeElapsed = m_duration;
            }
            Debug.Log($"Modifier::Apply {m_addingDamageResistance}% Dmg Resist Modifier");
        }
        
        public override void Update()
        {
            //Permanent type so duration will be -1, we no need to update
            if (m_duration == -1) return;
            
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
            m_playerController.ResistanceController.AddDamageResistance(-m_addingDamageResistance);
            Debug.Log($"Modifier::Remove {m_addingDamageResistance}% Dmg Resist Modifier");
        }
    }
}
