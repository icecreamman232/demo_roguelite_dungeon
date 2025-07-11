using SGGames.Script.Core;
using SGGames.Script.Events;
using UnityEngine;

namespace SGGames.Script.UI
{
    public class KeyUIController : MonoBehaviour
    {
        [SerializeField] private KeyUIView m_view;
        [SerializeField] private UpdateCurrencyUIEvent m_updateCurrencyUIEvent;

        private void Awake()
        {
            m_updateCurrencyUIEvent.AddListener(OnReceiveEvent);
            m_view.UpdateCoinView(0);
        }

        private void OnReceiveEvent(Global.ItemID item, int amount)
        {
            if (item == Global.ItemID.Key)
            {
                m_view.UpdateCoinView(amount);
            }
        }
    }
}