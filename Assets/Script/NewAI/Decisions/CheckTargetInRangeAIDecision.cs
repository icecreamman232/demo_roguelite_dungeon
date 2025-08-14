using SGGames.Scripts.AI;
using UnityEngine;

namespace SGGames.Script.AI
{
    public class CheckTargetInRangeAIDecision : AIDecision
    {
        [SerializeField] private float m_range;
        public override bool CheckDecision()
        {
            var distanceToTarget = Vector2.Distance(m_brain.Target.position, m_brain.transform.position);
            return distanceToTarget <= m_range;
        }
    }
}
