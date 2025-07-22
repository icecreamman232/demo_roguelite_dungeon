using SGGames.Script.Core;
using SGGames.Script.Events;
using UnityEngine;

namespace SGGames.Script.Pickable
{
    public class CurrencyTreasureChest : TreasureChest
    {
        [SerializeField] private CurrencyDropsEvent m_currencyDropsEvent;
        [SerializeField] private Global.ItemID m_currencyID;
        [SerializeField] private int m_baseMinAmount;
        [SerializeField] private int m_baseMaxAmount;
        
        private int GetBaseAmount() => Random.Range(m_baseMinAmount, m_baseMaxAmount);

        protected override void OpenChest()
        {
            m_currencyDropsEvent.Raise(m_currencyID,transform.position, GetBaseAmount());
            base.OpenChest();
        }
    }
}
