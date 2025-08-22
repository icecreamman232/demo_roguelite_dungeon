using SGGames.Scripts.EditorExtensions;
using SGGames.Scripts.Data;
using SGGames.Scripts.Items;
using UnityEngine;

namespace SGGames.Scripts.Data
{
    [CreateAssetMenu(fileName = "New Inventory Item Data", menuName = "SGGames/Data/Inventory Item")]
    public class InventoryItemData : ItemData
    {
        [SerializeField] [ShowProperties] private TriggerData m_triggerData;
        public TriggerData TriggerData => m_triggerData;
    }
}

