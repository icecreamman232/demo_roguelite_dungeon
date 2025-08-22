using SGGames.Scripts.Data;
using UnityEngine;

namespace SGGames.Scripts.Events
{
    [CreateAssetMenu(fileName = "Equip Inventory Item Event", menuName = "SGGames/Event/Equip Item")]
    public class EquipInventoryItemEvent : ScriptableEvent<InventoryItemData> { }
}
