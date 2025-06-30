using System;
using UnityEngine;

namespace SGGames.Script.Events
{
    [CreateAssetMenu(menuName = "SGGames/Event/Update Player Health", fileName = "New Update Player Health Event")]
    public class UpdatePlayerHealthEvent : ScriptableObject
    {
        private Action<float, float, bool> m_listener;

        public void AddListener(Action<float, float, bool> addListener)
        {
            m_listener += addListener;
        }

        public void RemoveListener(Action<float, float, bool> removeListener)
        {
            m_listener -= removeListener;
        }

        public void Raise(float currentHealth, float maxHealth, bool isInitialize)
        {
            m_listener?.Invoke(currentHealth, maxHealth, isInitialize);
        }
    }
}

