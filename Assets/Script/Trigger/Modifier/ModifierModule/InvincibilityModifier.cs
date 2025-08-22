using System;
using SGGames.Script.Items;
using SGGames.Scripts.Entities;
using UnityEngine;

namespace SGGames.Scripts.Items
{
    [Serializable]
    public class InvincibilityModifier : DurationBasedModifier
    {
        public InvincibilityModifier(PlayerController controller, float duration = 0) : base(controller, duration) { }
        
        public override void Apply()
        {
            if (m_entity.IsPlayer())
            {
                ((PlayerController) m_entity).Health.SetInvincibleByItem(true);
            }
            base.Apply();
            Debug.Log($"Modifier::Apply Invincibility Modifier");
        }
        
        public override void Remove()
        {
            if (m_entity.IsPlayer())
            {
                ((PlayerController) m_entity).Health.SetInvincibleByItem(false);
            }
            Debug.Log($"Modifier::Remove Invincibility Modifier");
        }
    }
}
