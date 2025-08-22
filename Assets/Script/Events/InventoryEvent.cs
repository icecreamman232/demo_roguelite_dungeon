using System;
using SGGames.Scripts.Core;
using SGGames.Scripts.Events;
using UnityEngine;

namespace SGGames.Scripts.Data
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

