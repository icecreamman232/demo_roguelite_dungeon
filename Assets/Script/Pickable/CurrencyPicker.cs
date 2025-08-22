using SGGames.Scripts.Core;
using SGGames.Scripts.Events;
using UnityEngine;

namespace SGGames.Scripts.Pickables
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class CurrencyPicker : ItemPicker
    {
        [SerializeField] private Global.ItemID m_currencyID;
        [SerializeField] private int m_amount;
        [SerializeField] private CurrencyEvent m_currencyEvent;
        public Global.ItemID ItemID => m_currencyID;
        public int Amount => m_amount;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;

            PickedUp();
            this.gameObject.SetActive(false);
        }
        

        protected override void OnReceiveGameEvent(Global.GameEventType eventType)
        {
            if (eventType == Global.GameEventType.RoomCreated)
            {
                if (this.gameObject.activeSelf)
                {
                    this.gameObject.SetActive(false);
                }
            }
        }

        protected override void PickedUp()
        {
            m_currencyEvent?.Raise(new CurrencyUpdateData
            {
                ItemID = m_currencyID,
                Amount = m_amount
            });
            base.PickedUp();
        }
    }
}
