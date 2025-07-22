using System.Collections.Generic;
using SGGames.Script.Core;
using SGGames.Script.Data;
using SGGames.Script.EditorExtensions;
using SGGames.Script.Events;
using SGGames.Script.Pickable;
using UnityEngine;

namespace SGGames.Script.Managers
{
    public class ItemEffectManager : MonoBehaviour, IGameService
    {
        [SerializeField] private EquipInventoryItemEvent m_equipInventoryItemEvent;
        [SerializeField] private List<ItemEffect> m_appliedItemEffects;

        private GameObject m_playerReference;
        
        private void Awake()
        {
            m_equipInventoryItemEvent.AddListener(OnReceiveEquipItemEvent);
            ServiceLocator.RegisterService<ItemEffectManager>(this);
            m_appliedItemEffects = new List<ItemEffect>();
            ConsoleCheatManager.RegisterCommands(this);
        }
        
        private void OnReceiveEquipItemEvent(InventoryItemData item)
        {
            if (item.ItemEffects == null || item.ItemEffects.Length == 0) return;
            
            foreach (var itemEffect in item.ItemEffects)
            {
                ApplyItemEffect(itemEffect);
            }
        }

        /// <summary>
        /// Apply given item effect to player
        /// </summary>
        /// <param name="itemEffect"></param>
        [CheatCode("item", "Apply item effect")]
        public void ApplyItemEffect(ItemEffect itemEffect)
        {
            var lvlManager = ServiceLocator.GetService<LevelManager>();
            m_appliedItemEffects.Add(itemEffect);
            itemEffect.ApplyEffect(lvlManager.Player);
            Debug.Log($"Applied effect {itemEffect.ItemEffectID}");
        }

        /// <summary>
        /// Remove item effect given to player
        /// </summary>
        /// <param name="itemEffect"></param>
        public void RemoveItemEffect(ItemEffect itemEffect)
        {
            var lvlManager = ServiceLocator.GetService<LevelManager>();
            m_appliedItemEffects.Remove(itemEffect);
            itemEffect.RemoveEffect(lvlManager.Player);
            Debug.Log($"Removed effect {itemEffect.ItemEffectID}");
        }
    }
}

