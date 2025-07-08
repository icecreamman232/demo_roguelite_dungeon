using System;
using System.Collections.Generic;
using SGGames.Script.Core;


namespace SGGames.Script.Managers
{
    /// <summary>
    /// Handle operation related to item that will be presented in main inventory
    /// </summary>
    [Serializable]
    public class Inventory
    {
        private Dictionary<int, Global.ItemID> m_inventoryDict = new Dictionary<int, Global.ItemID>();
        
        public bool AddItem(int slotIndex, Global.ItemID item)
        {
            return m_inventoryDict.TryAdd(slotIndex, item);
        }

        public void RemoveItem(int slotIndex)
        {
            m_inventoryDict.Remove(slotIndex);
        }

        public Global.ItemID GetItemAt(int slotIndex)
        {
            return m_inventoryDict[slotIndex];
        }

        public bool HasItem(int slotIndex)
        {
            return m_inventoryDict.ContainsKey(slotIndex);
        }

        public bool HasItem(Global.ItemID item)
        {
            return m_inventoryDict.ContainsValue(item);
        }

        public void ResetInventory()
        {
            m_inventoryDict.Clear();
        }
    }
}
