using System;
using SGGames.Script.Core;
using UnityEngine;

namespace SGGames.Script.Events
{
    [CreateAssetMenu(fileName = "Currency Drops Event", menuName = "SGGames/Event/Currency Drops ")]
    public class CurrencyDropsEvent : ScriptableObject
    {
        private Action<Global.ItemID, Vector3, int> m_listener;

        public virtual void AddListener(Action<Global.ItemID, Vector3, int> addListener)
        {
            m_listener += addListener;
        }

        public virtual void RemoveListener(Action<Global.ItemID, Vector3, int> removeListener)
        {
            m_listener -= removeListener;
        }

        public virtual void Raise(Global.ItemID itemID, Vector3 hostPosition, int amount)
        {
            m_listener?.Invoke(itemID, hostPosition, amount);
        }
    }
}
