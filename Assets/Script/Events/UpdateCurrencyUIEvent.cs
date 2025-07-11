using System;
using SGGames.Script.Core;
using UnityEngine;

namespace SGGames.Script.Events
{
    [CreateAssetMenu(menuName = "SGGames/Event/Update Currency", fileName = "New Update Currency Event")]
    public class UpdateCurrencyUIEvent : ScriptableObject
    {
        private Action<Global.ItemID, int> m_listener;

        public void AddListener(Action<Global.ItemID, int> addListener)
        {
            m_listener += addListener;
        }

        public void RemoveListener(Action<Global.ItemID, int> removeListener)
        {
            m_listener -= removeListener;
        }

        public void Raise(Global.ItemID item, int totalAmount)
        {
            m_listener?.Invoke(item, totalAmount);
        }
    }
}