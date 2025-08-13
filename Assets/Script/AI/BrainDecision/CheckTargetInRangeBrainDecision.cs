using SGGames.Scripts.AI;
using UnityEngine;

namespace SGGames.Script.AI
{
    public class CheckTargetInRangeBrainDecision : BrainDecision
    {
        [SerializeField] private int m_range;
        public override bool CheckDecision()
        {
            var currentDistance = Vector2.Distance(m_brain.transform.position, m_brain.Target.position);
            return currentDistance <= m_range;
        }
    }
}
