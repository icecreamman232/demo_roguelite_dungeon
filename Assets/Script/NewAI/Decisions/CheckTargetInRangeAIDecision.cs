using SGGames.Scripts.AI;
using UnityEngine;

namespace SGGames.Script.AI
{
    public class CheckTargetInRangeAIDecision : AIDecision
    {
        [SerializeField] private float m_range;
        public override bool CheckDecision()
        {
            var selfPosition = m_brain.Owner.transform.position;
            var targetPosition = m_brain.Target.position;
            
            var deltaX = Mathf.Abs(selfPosition.x - targetPosition.x);
            var deltaY = Mathf.Abs(selfPosition.y - targetPosition.y);
            
            bool isInHorizontalRange = deltaY <= 0.1f && deltaX <= m_range;
            bool isInVerticalRange = deltaX <= 0.1f && deltaY <= m_range;
            
            return isInHorizontalRange || isInVerticalRange;
        }
    }
}
