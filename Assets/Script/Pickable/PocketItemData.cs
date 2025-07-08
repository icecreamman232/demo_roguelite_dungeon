
using SGGames.Script.Core;
using UnityEngine;

namespace SGGames.Script.Data
{
    [CreateAssetMenu(fileName = "New Pocket Item Data", menuName = "SGGames/Data/Pocket Item")]
    public class PocketItemData : ItemData
    {
        [SerializeField] private PocketInventoryEvent m_pocketInventoryEvent;
        public override void Picked(int amount)
        {
            m_pocketInventoryEvent.Raise(Global.InventoryEventType.Add, m_itemID, amount);
            base.Picked(amount);
        }
    }
}
