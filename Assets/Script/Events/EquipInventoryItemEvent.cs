using SGGames.Script.Data;
using UnityEngine;

namespace SGGames.Script.Events
{
    [CreateAssetMenu(fileName = "Equip Inventory Item Event", menuName = "SGGames/Event/Equip Item")]
    public class EquipInventoryItemEvent : ScriptableEvent<InventoryItemData> { }
}
