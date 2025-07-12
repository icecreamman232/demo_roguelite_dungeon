using SGGames.Script.Core;
using UnityEngine;

namespace SGGames.Script.Data
{
    public class ItemData : ScriptableObject
    {
        [SerializeField] protected Sprite m_itemSprite;
        [SerializeField] protected Global.ItemID m_itemID;
        
        public Sprite ItemSprite => m_itemSprite;
        public Global.ItemID ItemID => m_itemID;
        
        public virtual void Picked(int amount)
        {
            
        }
    }
}

