using System;
using SGGames.Script.Core;
using SGGames.Script.Data;
using SGGames.Script.Events;
using UnityEngine;

namespace SGGames.Script.Managers
{
    public class InventoryManager : MonoBehaviour, IGameService
    {
        [SerializeField] private Inventory m_inventory;
        [SerializeField] private PocketInventory m_pocketInventory;
        [SerializeField] private InventoryEvent m_inventoryEvent;
        [SerializeField] private PocketInventoryEvent m_pocketInventoryEvent;
        [SerializeField] private UpdateCurrencyUIEvent m_updateCurrencyUIEvent;
        
        private void Awake()
        {
            ServiceLocator.RegisterService<InventoryManager>(this);
            m_inventory = new Inventory();
            m_pocketInventory = new PocketInventory();
            
            m_inventoryEvent.AddListener(OnReceiveInventoryEvent);
            m_pocketInventoryEvent.AddListener(OnReceivePocketInventoryEvent);
        }

        private void UpdateCurrencyUI(Global.ItemID itemID)
        {
            switch (itemID)
            {
                case Global.ItemID.Coin:
                    m_updateCurrencyUIEvent.Raise(itemID,m_pocketInventory.TotalCoin);
                    break;
                case Global.ItemID.Key:
                    m_updateCurrencyUIEvent.Raise(itemID,m_pocketInventory.TotalKey);
                    break;
            }
        }

        private void OnReceivePocketInventoryEvent(Global.InventoryEventType eventType,Global.ItemID itemID,int amount)
        {
            if(eventType == Global.InventoryEventType.Add)
            {
                m_pocketInventory.AddItem(itemID, amount);
                UpdateCurrencyUI(itemID);
            }
            else if (eventType == Global.InventoryEventType.Remove)
            {
                m_pocketInventory.Remove(itemID, amount);
                UpdateCurrencyUI(itemID);
            }
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
            return m_pocketInventory.TotalKey >= numberKey;
        }
    }
}

