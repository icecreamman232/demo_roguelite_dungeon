using SGGames.Scripts.AI;
using UnityEngine;


namespace SGGames.Script.AI
{
    public class TargetInRangeBrainDecision : BrainDecision
    {
        [SerializeField] private float m_range;
        [SerializeField] private LayerMask m_targetLayerMask;
        
        #if UNITY_EDITOR
        [SerializeField] private bool m_showDebug;
        #endif
     
        public override bool CheckDecision()
        {
            var target = Physics2D.OverlapCircle(transform.position, m_range, m_targetLayerMask);
            if (target != null)
            {
                m_brain.Target = target.transform;
                return true;
            }

            return false;
        }
        #if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (!m_showDebug) return;
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, m_range);
        }
        #endif
    }
}

