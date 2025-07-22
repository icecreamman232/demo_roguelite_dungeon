using SGGames.Script.Core;
using SGGames.Script.Data;
using SGGames.Script.Managers;
using SGGames.Script.Modules;
using UnityEngine;

namespace SGGames.Script.Pickable
{
    public class TreasureChest : MonoBehaviour, IInteractable
    {
        [SerializeField] protected bool m_isRequireKey;
        [SerializeField] protected int m_numberKeyRequired = 1;
        [SerializeField] protected PocketInventoryEvent m_pocketInventoryEvent;

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
                m_pocketInventoryEvent.Raise(Global.InventoryEventType.Remove, Global.ItemID.Key, m_numberKeyRequired);
                OpenChest();
            }
            else
            {
                OpenChest();
            }
        }
    }
}
