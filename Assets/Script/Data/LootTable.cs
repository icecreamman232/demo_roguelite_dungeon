using System;
using SGGames.Script.Core;
using UnityEngine;

namespace SGGames.Script.Data
{
    [CreateAssetMenu(menuName = "SGGames/Data/Loot Table", fileName = "New Loot Table")]
    public class LootTable : ScriptableObject
    {
        [SerializeField] private Loot[] m_lootTable;
        
        public Loot[] GetLootTable() => m_lootTable;
    }
    
    [Serializable]
    public class Loot
    {
        public Global.ItemID Item;
        public int MinAmount;
        public int MaxAmount;
    }
}

