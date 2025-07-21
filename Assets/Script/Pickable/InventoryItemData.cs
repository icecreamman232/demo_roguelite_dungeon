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
    }
}

