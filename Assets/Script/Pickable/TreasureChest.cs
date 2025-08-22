using SGGames.Script.Managers;
using SGGames.Scripts.Core;
using SGGames.Scripts.Events;
using SGGames.Scripts.Modules;
using UnityEngine;

namespace SGGames.Scripts.Pickable
{
    public class TreasureChest : MonoBehaviour, IInteractable
    {
        [SerializeField] protected bool m_isRequireKey;
        [SerializeField] protected int m_numberKeyRequired = 1;
        [SerializeField] protected CurrencyEvent m_currencyEvent;
        [SerializeField] protected GameEvent m_gameEvent;

        private void Awake()
        {
            m_gameEvent.AddListener(OnReceiveGameEvent);
        }

        private void OnDestroy()
        {
            m_gameEvent.RemoveListener(OnReceiveGameEvent);
        }

        private void OnReceiveGameEvent(Global.GameEventType eventType)
        {
            if (eventType == Global.GameEventType.RoomCreated)
            {
                Destroy(this.gameObject);
            }
        }

        protected virtual void OpenChest()
        {
            this.gameObject.SetActive(false);
        }
        
        public void Interact()
        {
            if (m_isRequireKey)
            {
                var inventoryManager = ServiceLocator.GetService<InventoryManager>();
                if (!inventoryManager.HasKeyNumber(m_numberKeyRequired)) return;
                m_currencyEvent.Raise(new CurrencyUpdateData
                {
                    ItemID = Global.ItemID.Key,
                    Amount = m_numberKeyRequired
                });
                OpenChest();
            }
            else
            {
                OpenChest();
            }
        }
    }
}
