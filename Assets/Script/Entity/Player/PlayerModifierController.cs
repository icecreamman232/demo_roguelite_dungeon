using System.Collections.Generic;
using SGGames.Script.Core;
using SGGames.Script.Entities;
using SGGames.Script.Items;
using UnityEngine;


namespace SGGames.Script.Entity
{
    public class PlayerModifierController : EntityBehavior, IModifierController
    {
        [SerializeField] private WorldEvent m_worldEvent;
        private PlayerController m_controller;
        /// <summary>
        /// List of modifier that will be removed when their live time is over
        /// </summary>
        private List<Modifier> m_durationBasedModifiers;
        
        /// <summary>
        /// List of modifier that will be removed when there's specific event triggered
        /// </summary>
        private List<Modifier> m_eventBasedModifier;

        private void Start()
        {
            m_durationBasedModifiers = new List<Modifier>();
            m_eventBasedModifier = new List<Modifier>();
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
            m_durationBasedModifiers.Add(modifier);
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
            
        }
    }
}
