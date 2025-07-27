using System;
using SGGames.Script.Core;
using UnityEngine;

namespace SGGames.Script.Managers
{
    /// <summary>
    /// Handles all operation on type of item which will be not presented in main inventory such as: key, coin, bomb etc...
    /// </summary>
    [Serializable]
    public class CurrencyInventory
    {
        [SerializeField] private int m_totalCoin = 0;
        [SerializeField] private int m_totalKey = 0;
        
        public int TotalCoin => m_totalCoin;
        public int TotalKey => m_totalKey;
        
        public void AddItem(Global.ItemID id, int amount)
        {
            switch (id)
            {
                case Global.ItemID.Coin:
                    m_totalCoin += amount;
                    break;
                case Global.ItemID.Key:
                    m_totalKey += amount;
                    break;
            }
        }
        
        public void ResetInventory()
        {
            m_totalCoin = 0;
            m_totalKey = 0;
        }
    }
}
