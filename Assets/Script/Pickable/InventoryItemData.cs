using SGGames.Script.Core;
using UnityEngine;

namespace SGGames.Script.Data
{
    [CreateAssetMenu(fileName = "New Pocket Item Data", menuName = "SGGames/Data/Inventory Item")]
    public class InventoryItemData : ItemData
    {
        [SerializeField] private InventoryEvent m_inventoryEvent;

        public override void Picked(int amount)
        {
            //Slot index -1 because we dont know which slot the item will go to. The default is an empty slot.
            m_inventoryEvent.Raise(Global.InventoryEventType.Add, m_itemID, amount);
            base.Picked(amount);
        }
    }
}

