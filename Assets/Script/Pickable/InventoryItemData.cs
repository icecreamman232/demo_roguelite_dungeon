using SGGames.Script.Core;
using SGGames.Script.Managers;
using SGGames.Script.Pickable;
using UnityEngine;

namespace SGGames.Script.Data
{
    [CreateAssetMenu(fileName = "New Pocket Item Data", menuName = "SGGames/Data/Inventory Item")]
    public class InventoryItemData : ItemData
    {
        [SerializeField] private ItemEffect[] m_itemEffects;
        [SerializeField] private InventoryEvent m_inventoryEvent;

        public ItemEffect[] ItemEffects => m_itemEffects;
        
        public override void Picked(int amount)
        {
            Debug.Log($"Picked {m_itemID}");
            m_inventoryEvent.Raise(Global.InventoryEventType.Add, m_itemID, amount);
            var itemEffectManager = ServiceLocator.GetService<ItemEffectManager>();
            foreach (var itemEffect in m_itemEffects)
            {
                itemEffectManager.ApplyItemEffect(itemEffect);
            }
            
            base.Picked(amount);
        }
    }
}

