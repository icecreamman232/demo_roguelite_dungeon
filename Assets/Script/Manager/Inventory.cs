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
        private Dictionary<Global.ItemID, int> m_inventoryDict = new Dictionary<Global.ItemID, int>();
        
        public bool AddItem(Global.ItemID item, int amount)
        {
            return m_inventoryDict.TryAdd(item, amount);
        }

        public void RemoveItem(Global.ItemID item)
        {
            m_inventoryDict.Remove(item);
        }
        
        public bool HasItem(Global.ItemID item)
        {
            return m_inventoryDict.ContainsKey(item);
        }
        
        public void ResetInventory()
        {
            m_inventoryDict.Clear();
        }
    }
}
