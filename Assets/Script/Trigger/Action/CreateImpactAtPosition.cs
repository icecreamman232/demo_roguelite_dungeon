using SGGames.Scripts.Skills;
using UnityEngine;

namespace SGGames.Scripts.Items
{
    [CreateAssetMenu(fileName = "Create Impact At Position", menuName = "SGGames/Trigger/Action/Create Impact At Position")]
    public class CreateImpactAtPosition : TriggerAction
    {
        [SerializeField] private ImpactParamInfo m_impactParamInfo;
        [SerializeField] private TriggerImpactEvent m_impactEvent;
        [SerializeField] private bool m_isCreateAtSourcePosition;
    
        public override void Execute(GameObject source, GameObject target)
        {
            m_impactEvent.Raise(m_isCreateAtSourcePosition 
                    ? source.transform.position 
                    : target.transform.position,
                m_impactParamInfo);
        }
    }
}
