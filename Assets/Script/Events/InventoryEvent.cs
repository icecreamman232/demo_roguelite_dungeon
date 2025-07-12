using System;
using SGGames.Script.Core;
using UnityEngine;

namespace SGGames.Script.Data
{
    [CreateAssetMenu(fileName = "Inventory Event", menuName = "SGGames/Event/Inventory")]
    public class InventoryEvent: ScriptableObject
    {
        protected Action<Global.InventoryEventType,Global.ItemID, int> m_listener;

        public virtual void AddListener(Action<Global.InventoryEventType,Global.ItemID, int> addListener)
        {
            m_listener += addListener;
        }

        public virtual void RemoveListener(Action<Global.InventoryEventType,Global.ItemID, int> removeListener)
        {
            m_listener -= removeListener;
        }

        public virtual void Raise(Global.InventoryEventType inventoryEventType,Global.ItemID item, int amount)
        {
            m_listener?.Invoke(inventoryEventType,item, amount);
        }
    }
}

