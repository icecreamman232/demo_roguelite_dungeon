using System.Collections.Generic;
using SGGames.Script.Items;
using UnityEngine;

namespace SGGames.Script.Core
{
    public class TriggerManager : MonoBehaviour, IGameService
    {
        [SerializeField] private WorldEvent m_worldEvent;
        [SerializeField] private List<TriggerData> m_triggerDataList;
        
        private void Awake()
        {
            ServiceLocator.RegisterService<TriggerManager>(this);
            m_worldEvent.AddListener(OnReceiveWorldEvent);
        }

        private void OnDestroy()
        {
            m_worldEvent.RemoveListener(OnReceiveWorldEvent);
            ServiceLocator.UnregisterService<TriggerManager>();
        }
        
        public void AddTrigger(TriggerData triggerData)
        {
            m_triggerDataList.Add(triggerData);
        }
        
        private void OnReceiveWorldEvent(Global.WorldEventType eventType, GameObject source, GameObject target)
        {
            //Debug.Assert(eventType == Global.WorldEventType.OnPlayerPerfectDodge, "Player Did Perfect Dodge");
            
            foreach (var triggerData in m_triggerDataList)
            {
                if (triggerData.CheckEvent(eventType))
                {
                    triggerData.Execute(eventType, source, target);
                }
            }
        }

    }
}
