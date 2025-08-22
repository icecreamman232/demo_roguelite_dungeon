using SGGames.Scripts.Core;
using UnityEngine;

namespace SGGames.Scripts.Skills
{
    public class ImpactManager : MonoBehaviour, IGameService
    {
        [SerializeField] private TriggerImpactEvent m_impactEvent;
        [SerializeField] private StunAOEIMpact m_stunAOEImpactPrefab;
        
        private void Awake()
        {
            ServiceLocator.RegisterService<ImpactManager>(this);
            m_impactEvent.AddListener(OnReceiveImpactEvent);
        }

        private void OnDestroy()
        {
            m_impactEvent.RemoveListener(OnReceiveImpactEvent);
        }

        private void OnReceiveImpactEvent(Vector3 position, ImpactParamInfo paramInfo)
        {
            switch (paramInfo.ImpactID)
            {
                case Global.ImpactID.StunAOE:
                    var stunAOEImpact = Instantiate(m_stunAOEImpactPrefab, position, Quaternion.identity);
                    stunAOEImpact.Initialize(paramInfo as StunAOEParamInfo);
                    stunAOEImpact.Execute();
                    break;
            }
        }
    }
}
