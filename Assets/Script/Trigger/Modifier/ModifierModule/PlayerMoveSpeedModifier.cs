using System;

namespace SGGames.Scripts.Items
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
    }
}

