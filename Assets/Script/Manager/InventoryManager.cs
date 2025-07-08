using SGGames.Script.Core;
using SGGames.Script.Data;
using UnityEngine;

namespace SGGames.Script.Managers
{
    public class InventoryManager : MonoBehaviour, IGameService
    {
        [SerializeField] private Inventory m_inventory;
        [SerializeField] private PocketInventory m_pocketInventory;
        [SerializeField] private InventoryEvent m_inventoryEvent;
        [SerializeField] private PocketInventoryEvent m_pocketInventoryEvent;
        
        private void Awake()
        {
            ServiceLocator.RegisterService<InventoryManager>(this);
            m_inventory = new Inventory();
            m_pocketInventory = new PocketInventory();
            
            m_inventoryEvent.AddListener(OnReceiveInventoryEvent);
            m_pocketInventoryEvent.AddListener(OnReceivePocketInventoryEvent);
        }

        private void OnReceivePocketInventoryEvent(Global.InventoryEventType eventType,Global.ItemID itemID,int amount)
        {
            if(eventType == Global.InventoryEventType.Add)
            {
                m_pocketInventory.AddItem(itemID, amount);
            }
            else if (eventType == Global.InventoryEventType.Remove)
            {
                m_pocketInventory.Remove(itemID, amount);
            }
        }

        private void OnReceiveInventoryEvent(Global.InventoryEventType eventType, int slotIndex, Global.ItemID itemID)
        {
            if(eventType == Global.InventoryEventType.Add)
            {
                m_inventory.AddItem(slotIndex, itemID);
            }
            else if (eventType == Global.InventoryEventType.Remove)
            {
                m_inventory.RemoveItem(slotIndex);
            }
        }
    }
}

