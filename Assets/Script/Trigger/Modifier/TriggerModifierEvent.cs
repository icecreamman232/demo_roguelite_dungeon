using System;
using UnityEngine;

namespace SGGames.Script.Items
{
    [CreateAssetMenu(menuName = "SGGames/Event/Trigger Modifier", fileName = "Trigger Modifier Event")]
    public class TriggerModifierEvent : ScriptableObject
    {
        private Action<ModifierData> m_listener;

        public void AddListener(Action<ModifierData> addListener)
        {
            m_listener += addListener;
        }

        public void RemoveListener(Action<ModifierData> removeListener)
        {
            m_listener -= removeListener;
        }

        public void Raise(ModifierData data)
        {
            m_listener?.Invoke(data);
        }
    }
}