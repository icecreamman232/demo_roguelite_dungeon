using System;
using SGGames.Script.Entity;

namespace SGGames.Script.Items
{
    public class PlayerMoveSpeedModifier : DurationBasedModifier
    {
        private bool m_isFlatValue;
        private float m_bonusSpeed;
        private Guid m_guid;
    
        public PlayerMoveSpeedModifier(IEntityIdentifier controller, bool isFlatValue, float bonusSpeed, float duration) : base(controller, duration)
        {
            m_guid = Guid.NewGuid();
            m_isFlatValue = isFlatValue;
            m_bonusSpeed = bonusSpeed;
        }

        public override void Apply()
        {
            if (m_entity.IsPlayer())
            {
                var playerMovement = ((PlayerController)m_entity).PlayerMovement;
                if (m_isFlatValue)
                {
                    playerMovement.AddFlatSpeedBonus(m_bonusSpeed);
                }
                else
                {
                    playerMovement.AddPercentageSpeedBonus(m_guid, m_bonusSpeed);
                }
            }
            base.Apply();
        }

        public override void Remove()
        {
            var playerMovement = ((PlayerController)m_entity).PlayerMovement;
            if (m_isFlatValue)
            {
                playerMovement.RemoveFlatSpeedBonus(m_bonusSpeed);
            }
            else
            {
                playerMovement.RemovePercentageSpeedBonus(m_guid);
            }
            base.Remove();
        }
    }

}

