using System;
using SGGames.Script.Core;
using UnityEngine;

namespace SGGames.Script.Events
{
    [CreateAssetMenu(fileName = "Currency Event", menuName = "SGGames/Event/Currency")]
    public class CurrencyEvent : ScriptableObject
    {
        private Action<Global.ItemID, int> m_listener;

        public virtual void AddListener(Action<Global.ItemID, int> addListener)
        {
            m_listener += addListener;
        }

        public virtual void RemoveListener(Action<Global.ItemID, int> removeListener)
        {
            m_listener -= removeListener;
        }

        public virtual void Raise(Global.ItemID item, int amount)
        {
            m_listener?.Invoke(item, amount);
        }
    }
}
