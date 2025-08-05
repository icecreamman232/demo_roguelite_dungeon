using System;
using System.Collections.Generic;

namespace SGGames.Script.Modules
{
    public class PercentageStackInstance
    {
        public Guid Guid { get; private set; }
        public float Percentage { get; private set; }

        public PercentageStackInstance(Guid guid, float percentage)
        {
            Guid = guid;
            Percentage = percentage;
        }
    }
    
    public class PercentageStackController
    {
        private List<PercentageStackInstance> m_percentageStacks;

        public PercentageStackController()
        {
            m_percentageStacks = new List<PercentageStackInstance>();
        }
        
        public void AddPercentage(Guid guid, float percentage)
        {
            m_percentageStacks.Add(new PercentageStackInstance(guid, percentage));
        }

        public void RemovePercentage(Guid guid)
        {
            var percentageStack = m_percentageStacks.Find(x => x.Guid == guid);
            m_percentageStacks.Remove(percentageStack);
        }

        public float GetValueWithPercentageStack(float value)
        {
            var finalValue = value;
            foreach (var percentage in m_percentageStacks)
            {
                finalValue = finalValue * (1 + percentage.Percentage/100);
            }
            return finalValue;
        }
    }
}
