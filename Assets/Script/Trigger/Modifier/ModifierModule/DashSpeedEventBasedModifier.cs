using System;
using SGGames.Script.Core;
using SGGames.Script.Entity;

namespace SGGames.Script.Items
{
    public class DashSpeedEventBasedModifier : EventBasedModifier
    {
        private Guid m_guid;
        private float m_bonusSpeed;
        private bool m_isFlatValue;
        
        public Guid Guid => m_guid;
    
        public DashSpeedEventBasedModifier(IEntityIdentifier controller, Global.WorldEventType eventType, int numberTrigger, bool isFlatValue, float bonusSpeed) 
            : base(controller, eventType, numberTrigger)
        {
            m_guid = Guid.NewGuid();
            m_isFlatValue = isFlatValue;
            m_bonusSpeed = bonusSpeed;
        }

        public override void Apply()
        {
            if (m_entity.IsPlayer())
            {
                var playerDash = ((PlayerController)m_entity).PlayerDash;
                if (m_isFlatValue)
                {
                    playerDash.AddFlatBonusSpeedFromItem(m_bonusSpeed);
                }
                else
                {
                    playerDash.AddPercentageBonusSpeedFromItem(m_guid, m_bonusSpeed);
                }
            }
        
            base.Apply();
        }

        public override void Remove()
        {
            var playerDash = ((PlayerController)m_entity).PlayerDash;
            if (m_isFlatValue)
            {
                playerDash.RemoveFlatBonusSpeedFromItem(m_bonusSpeed);
            }
            else
            {
                playerDash.RemovePercentageBonusSpeedFromItem(m_guid);
            }
            base.Remove();
        }
    }
}
