using System;
using SGGames.Script.Core;
using SGGames.Script.Events;
using UnityEngine;

namespace SGGames.Script.UI
{
    public class CoinUIController : MonoBehaviour
    {
        [SerializeField] private CoinUIView m_view;
        [SerializeField] private UpdateCurrencyUIEvent m_updateCurrencyUIEvent;

        private void Awake()
        {
            m_updateCurrencyUIEvent.AddListener(OnReceiveEvent);
            m_view.UpdateCoinView(0);
        }

        private void OnReceiveEvent(CurrencyUpdateData currencyUpdateData)
        {
            if (currencyUpdateData.ItemID == Global.ItemID.Coin)
            {
                m_view.UpdateCoinView(currencyUpdateData.Amount);
            }
        }

        private void OnDestroy()
        {
            m_updateCurrencyUIEvent.RemoveListener(OnReceiveEvent);
        }
    }
}
