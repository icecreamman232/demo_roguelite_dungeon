using SGGames.Scripts.AI;
using UnityEngine;

namespace SGGames.Script.AI
{
    /// <summary>
    /// Return true if Time in State is equal or greater random time
    /// </summary>
    public class TimeInStateBrainDecision : BrainDecision
    {
        [SerializeField] private float m_minDuration;
        [SerializeField] private float m_maxDuration;
        
        private float m_waitTime;
        
        public override void OnEnterState()
        {
            m_waitTime = Random.Range(m_minDuration, m_maxDuration);
            base.OnEnterState();
        }

        public override bool CheckDecision()
        {
            return m_brain.TimeInState >= m_waitTime;
        }
    }
}

