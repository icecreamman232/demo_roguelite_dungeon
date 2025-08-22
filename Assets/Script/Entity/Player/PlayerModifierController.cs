using System.Collections.Generic;
using SGGames.Scripts.Core;
using SGGames.Script.Items;
using SGGames.Scripts.Items;
using UnityEngine;


namespace SGGames.Scripts.Entities
{
    public class PlayerModifierController : EntityBehavior, IModifierController
    {
        [SerializeField] private WorldEvent m_worldEvent;
        private PlayerController m_controller;
        /// <summary>
        /// List of modifier that will be removed when their live time is over
        /// </summary>
        private List<DurationBasedModifier> m_durationBasedModifiers;
        
        /// <summary>
        /// List of modifier that will be removed when there's specific event triggered
        /// </summary>
        private List<EventBasedModifier> m_eventBasedModifier;

        private void Start()
        {
            m_durationBasedModifiers = new List<DurationBasedModifier>();
            m_eventBasedModifier = new List<EventBasedModifier>();
            m_worldEvent.AddListener(OnReceiveWorldEvent);
        }

        private void OnDestroy()
        {
            m_worldEvent.RemoveListener(OnReceiveWorldEvent);
        }

        private void Update()
        {
            UpdateModifiers();
        }

        public void Initialize(PlayerController controller)
        {
            m_controller = controller;
        }


        public void AddModifier(ModifierData modifierData)
        {
            var modifier = ModifierFactory.CreateModifier(m_controller, modifierData);
            if (modifier is DurationBasedModifier)
            {
                m_durationBasedModifiers.Add((DurationBasedModifier)modifier);
            }
            else
            {
                m_eventBasedModifier.Add((EventBasedModifier)modifier);
            }
            
            modifier.Apply();
        }
        

        public void UpdateModifiers()
        {
            if (m_durationBasedModifiers.Count > 0)
            {
                for (int i = m_durationBasedModifiers.Count - 1; i >= 0; i--)
                {
                    m_durationBasedModifiers[i].Update();
                    if (m_durationBasedModifiers[i].CanRemove)
                    {
                        m_durationBasedModifiers[i].Remove();
                        m_durationBasedModifiers.RemoveAt(i);
                    }
                }
            }
        }
        
        private void OnReceiveWorldEvent(Global.WorldEventType eventType, GameObject source, GameObject target)
        {
            foreach (var modifier in m_eventBasedModifier)
            {
                modifier.CheckEvent(eventType);
            }
        }
    }
}
