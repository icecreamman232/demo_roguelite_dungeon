using System;
using SGGames.Script.Core;
using UnityEngine;

namespace SGGames.Script.Data
{
    [CreateAssetMenu(fileName = "Inventory Event", menuName = "SGGames/Event/Inventory")]
    public class InventoryEvent: ScriptableObject
    {
        protected Action<Global.InventoryEventType, int, Global.ItemID> m_listener;

        public virtual void AddListener(Action<Global.InventoryEventType, int, Global.ItemID> addListener)
        {
            m_listener += addListener;
        }

        public virtual void RemoveListener(Action<Global.InventoryEventType, int, Global.ItemID> removeListener)
        {
            m_listener -= removeListener;
        }

        public virtual void Raise(Global.InventoryEventType inventoryEventType, int slotIndex, Global.ItemID item)
        {
            m_listener?.Invoke(inventoryEventType, slotIndex, item);
        }
    }
}

