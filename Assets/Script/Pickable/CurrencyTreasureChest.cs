using SGGames.Script.Pickable;
using SGGames.Scripts.Pickable;
using SGGames.Scripts.Core;
using SGGames.Scripts.Events;
using UnityEngine;

namespace SGGames.Scripts.Pickable
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
            m_currencyDropsEvent.Raise(new CurrencyDropData
            {
                ItemID = m_currencyID,
                HostPosition = transform.position,
                Amount = GetBaseAmount()
            });
            base.OpenChest();
        }
    }
}
