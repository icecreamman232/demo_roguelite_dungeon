using SGGames.Script.Core;
using SGGames.Script.Data;
using SGGames.Script.Events;
using UnityEngine;

namespace SGGames.Script.Managers
{
    public class InventoryManager : MonoBehaviour, IGameService
    {
        [SerializeField] private Inventory m_inventory;
        [SerializeField] private CurrencyInventory currencyInventory;
        [SerializeField] private InventoryEvent m_inventoryEvent;
        [SerializeField] private CurrencyEvent m_currencyEvent;
        [SerializeField] private UpdateCurrencyUIEvent m_updateCurrencyUIEvent;
        
        private void Awake()
        {
            ServiceLocator.RegisterService<InventoryManager>(this);
            m_inventory = new Inventory();
            currencyInventory = new CurrencyInventory();
            
            m_inventoryEvent.AddListener(OnReceiveInventoryEvent);
            m_currencyEvent.AddListener(OnReceiveCurrencyEvent);
        }

        private void UpdateCurrencyUI(Global.ItemID itemID)
        {
            switch (itemID)
            {
                case Global.ItemID.Coin:
                    m_updateCurrencyUIEvent.Raise(itemID,currencyInventory.TotalCoin);
                    break;
                case Global.ItemID.Key:
                    m_updateCurrencyUIEvent.Raise(itemID,currencyInventory.TotalKey);
                    break;
            }
        }

        private void OnReceiveCurrencyEvent(Global.ItemID itemID,int amount)
        {
            currencyInventory.AddItem(itemID, amount);
            UpdateCurrencyUI(itemID);
        }

        private void OnReceiveInventoryEvent(Global.InventoryEventType eventType,Global.ItemID itemID, int amount)
        {
            if(eventType == Global.InventoryEventType.Add)
            {
                m_inventory.AddItem(itemID, amount );
            }
            else if (eventType == Global.InventoryEventType.Remove)
            {
                m_inventory.RemoveItem(itemID);
            }
        }

        public bool HasKeyNumber(int numberKey)
        {
            return currencyInventory.TotalKey >= numberKey;
        }
    }
}

