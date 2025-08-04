using SGGames.Script.EditorExtensions;
using UnityEngine;

namespace SGGames.Script.Data
{
    [CreateAssetMenu(fileName = "New Inventory Item Data", menuName = "SGGames/Data/Inventory Item")]
    public class InventoryItemData : ItemData
    {
        [SerializeField] [ShowProperties] private TriggerData m_triggerData;
        public TriggerData TriggerData => m_triggerData;
    }
}

