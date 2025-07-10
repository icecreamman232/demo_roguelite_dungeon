using System;
using UnityEngine;

namespace SGGames.Script.Events
{
    [CreateAssetMenu(menuName = "SGGames/Event/Update Player Stamina", fileName = "New Update Player Stamina Event")]
    public class UpdatePlayerStaminaEvent : ScriptableObject
    {
        private Action<int, int, bool> m_listener;

        public void AddListener(Action<int, int, bool> addListener)
        {
            m_listener += addListener;
        }

        public void RemoveListener(Action<int, int, bool> removeListener)
        {
            m_listener -= removeListener;
        }

        public void Raise(int currentStamina, int maxStamina, bool isInitialize)
        {
            m_listener?.Invoke(currentStamina, maxStamina, isInitialize);
        }
    }

}
