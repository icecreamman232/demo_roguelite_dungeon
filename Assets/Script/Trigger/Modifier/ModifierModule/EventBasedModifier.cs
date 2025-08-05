using SGGames.Script.Core;

namespace SGGames.Script.Items
{
    public class EventBasedModifier : Modifier
    {
        protected int m_numberTriggerBeforeRemove;
        protected Global.WorldEventType m_eventType;

        public EventBasedModifier(IEntityIdentifier controller, Global.WorldEventType eventType, int numberTrigger) : base(controller)
        {
            m_eventType = eventType;
            m_numberTriggerBeforeRemove = numberTrigger;
        }

        public override void Apply()
        {
            
        }

        public override void Remove()
        {
        
        }

        public virtual void CheckEvent(Global.WorldEventType eventType)
        {
            if (eventType == m_eventType)
            {
                m_numberTriggerBeforeRemove--;
                if (m_numberTriggerBeforeRemove <= 0)
                {
                    m_shouldBeRemoved = true;
                    Remove();
                }
            }
        }
    }
}
