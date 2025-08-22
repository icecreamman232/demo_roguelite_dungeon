using SGGames.Scripts.Core;
using SGGames.Scripts.Data;
using SGGames.Scripts.Events;
using SGGames.Scripts.Managers;
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

        private void OnDestroy()
        {
            m_inventoryEvent.RemoveListener(OnReceiveInventoryEvent);
            m_currencyEvent.RemoveListener(OnReceiveCurrencyEvent);
        }

        private void UpdateCurrencyUI(Global.ItemID itemID)
        {
            switch (itemID)
            {
                case Global.ItemID.Coin:
                    m_updateCurrencyUIEvent.Raise(new CurrencyUpdateData
                    {
                        ItemID = itemID,
                        Amount = currencyInventory.TotalCoin
                    });
                    break;
                case Global.ItemID.Key:
                    m_updateCurrencyUIEvent.Raise(new CurrencyUpdateData
                    {
                        ItemID = itemID,
                        Amount = currencyInventory.TotalKey
                    });
                    break;
            }
        }

        private void OnReceiveCurrencyEvent(CurrencyUpdateData currencyUpdateData)
        {
            currencyInventory.AddItem(currencyUpdateData.ItemID, currencyUpdateData.Amount);
            UpdateCurrencyUI(currencyUpdateData.ItemID);
        }

        private void OnReceiveInventoryEvent(InventoryEventData inventoryEventData)
        {
            if(inventoryEventData.InventoryEventType == Global.InventoryEventType.Add)
            {
                m_inventory.AddItem(inventoryEventData.ItemID, inventoryEventData.Amount);
            }
            else if (inventoryEventData.InventoryEventType == Global.InventoryEventType.Remove)
            {
                m_inventory.RemoveItem(inventoryEventData.ItemID);
            }
        }

        public bool HasKeyNumber(int numberKey)
        {
            return currencyInventory.TotalKey >= numberKey;
        }
    }
}

