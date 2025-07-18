using SGGames.Scripts.AI;
using SGGames.Scripts.Entity;
using UnityEngine;

namespace SGGames.Script.AI
{
    public class SwitchBrainBrainAction : BrainAction
    {
        [SerializeField] private EnemyBrain m_brainToSwap;
        [SerializeField] private bool m_keepTarget;

        public override void DoAction()
        {
            m_brain.ActivateBrain(false);
            m_brain.ResetBrain();
            m_brain.gameObject.SetActive(false);

            if (m_keepTarget)
            {
                m_brainToSwap.Target = m_brain.Target;
            }

            m_brainToSwap.Owner = m_brain.Owner;
            m_brainToSwap.Owner.SetActiveBrain(m_brainToSwap);
            m_brainToSwap.gameObject.SetActive(true);
            m_brainToSwap.ActivateBrain(true);
        }
    }

}
