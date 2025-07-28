using System.Collections.Generic;
using SGGames.Script.Core;
using SGGames.Script.EditorExtensions;
using SGGames.Script.Skills;
using UnityEngine;

namespace SGGames.Script.Data
{
    [CreateAssetMenu(fileName = "New Inventory Item Data", menuName = "SGGames/Data/Inventory Item")]
    public class InventoryItemData : ItemData
    {
        [Tooltip("Player event required to trigger this item")]
        [SerializeField] private List<Global.PlayerEvents> m_playerEventTrigger;
        [ShowProperties]
        [SerializeField] private List<ModifierData> m_modifierData;
        [ShowProperties]
        [SerializeField] private List<ImpactParamInfo> m_impactParamInfo;
        
        public List<Global.PlayerEvents> PlayerEventTrigger => m_playerEventTrigger;
        public List<ModifierData> ModifierData => m_modifierData;
        public List<ImpactParamInfo> ImpactParamInfo => m_impactParamInfo;
    }
}

