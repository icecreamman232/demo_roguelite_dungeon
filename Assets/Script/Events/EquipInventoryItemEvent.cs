using System;
using SGGames.Script.Core;
using SGGames.Script.Data;
using UnityEngine;

namespace SGGames.Script.Events
{
    [CreateAssetMenu(fileName = "Equip Inventory Item Event", menuName = "SGGames/Event/Equip Item")]
    public class EquipInventoryItemEvent : ScriptableObject
    {
        private Action<InventoryItemData> m_listener;

        public void AddListener(Action<InventoryItemData> addListener)
        {
            m_listener += addListener;
        }

        public void RemoveListener(Action<InventoryItemData> removeListener)
        {
            m_listener -= removeListener;
        }

        public void Raise(InventoryItemData item)
        {
            m_listener?.Invoke(item);
        }
    }
}
