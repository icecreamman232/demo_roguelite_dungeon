using SGGames.Script.Core;
using UnityEngine;

namespace SGGames.Script.Data
{
    public class ItemData : ScriptableObject
    {
        [SerializeField] protected Global.ItemID m_itemID;
        
        public Global.ItemID ItemID => m_itemID;
        
        public virtual void Picked(int amount)
        {
            
        }
    }
}

