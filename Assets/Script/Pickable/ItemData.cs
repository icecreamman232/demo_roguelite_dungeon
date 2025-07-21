using SGGames.Script.Core;
using UnityEngine;

namespace SGGames.Script.Data
{
    public class ItemData : ScriptableObject
    {
        [SerializeField] protected Global.ItemRarity m_rarity;
        [SerializeField] protected Sprite m_itemSprite;
        [SerializeField] protected Global.ItemID m_itemID;
        
        public Global.ItemRarity Rarity => m_rarity;
        public Sprite ItemSprite => m_itemSprite;
        public Global.ItemID ItemID => m_itemID;
    }
}

