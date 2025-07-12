using System.Collections.Generic;
using SGGames.Script.Core;
using SGGames.Script.Pickable;
using UnityEngine;

namespace SGGames.Script.Managers
{
    public class ItemEffectManager : MonoBehaviour, IGameService
    {
        [SerializeField] private List<ItemEffect> m_appliedItemEffects;

        private GameObject m_playerReference;
        
        private void Awake()
        {
            ServiceLocator.RegisterService<ItemEffectManager>(this);
            m_appliedItemEffects = new List<ItemEffect>();
        }
        /// <summary>
        /// Apply given item effect to player
        /// </summary>
        /// <param name="itemEffect"></param>
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

