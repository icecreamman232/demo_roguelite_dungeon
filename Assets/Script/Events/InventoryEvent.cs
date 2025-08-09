using System;
using SGGames.Script.Core;
using SGGames.Script.Events;
using UnityEngine;

namespace SGGames.Script.Data
{
    [CreateAssetMenu(fileName = "Inventory Event", menuName = "SGGames/Event/Inventory")]
    public class InventoryEvent: ScriptableEvent<InventoryEventData> { }
    
    [Serializable]
    public class InventoryEventData
    {
        public Global.InventoryEventType InventoryEventType;
        public Global.ItemID ItemID;
        public int Amount;
    }
}

