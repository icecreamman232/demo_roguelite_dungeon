using System;
using UnityEngine;

namespace SGGames.Script.Events
{
    [CreateAssetMenu(menuName = "SGGames/Event/Update Player Dmg Proj", fileName = "New Update Player Dmg Proj Event")]
    public class UpdatePlayerProjectileDamageEvent : ScriptableObject
    {
        private Action<float> m_listener;

        public void AddListener(Action<float> addListener)
        {
            m_listener += addListener;
        }

        public void RemoveListener(Action<float> removeListener)
        {
            m_listener -= removeListener;
        }

        public void Raise(float modifierValue)
        {
            m_listener?.Invoke(modifierValue);
        }
    }
}