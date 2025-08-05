using SGGames.Script.Skills;
using UnityEngine;

namespace SGGames.Script.Items
{
    public class DurationBasedModifier :Modifier
    {
        protected float m_duration;
        protected float m_timeElapsed;
        
        public DurationBasedModifier(IEntityIdentifier controller, float duration) : base(controller)
        {
            m_duration = duration;
        }

        public override void Apply()
        {
            if (m_duration > 0)
            {
                m_timeElapsed = m_duration;
            }
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
            
        }
    }
}
