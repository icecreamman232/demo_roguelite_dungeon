using System;
using SGGames.Scripts.Entities;
using SGGames.Scripts.Items;
using UnityEngine;

namespace SGGames.Script.Items
{
    [Serializable]
    public class DamageResistanceModifier : DurationBasedModifier
    {
        private float m_addingDamageResistance;
        
        public DamageResistanceModifier(PlayerController controller, float addingDamageResistance, float duration)
            : base(controller, duration)
        {
            m_addingDamageResistance = addingDamageResistance;
        }

        public override void Apply()
        {
            if (m_entity.IsPlayer())
            {
                ((PlayerController) m_entity).ResistanceController.AddDamageResistance(m_addingDamageResistance);
            }
            base.Apply();
            Debug.Log($"Modifier::Apply {m_addingDamageResistance}% Dmg Resist Modifier");
        }
        
        public override void Remove()
        {
            if (m_entity.IsPlayer())
            {
                ((PlayerController) m_entity).ResistanceController.AddDamageResistance(-m_addingDamageResistance);
            }
            Debug.Log($"Modifier::Remove {m_addingDamageResistance}% Dmg Resist Modifier");
        }
    }
}
