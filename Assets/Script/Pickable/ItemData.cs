using SGGames.Script.Core;
using UnityEngine;

namespace SGGames.Script.Data
{
    [CreateAssetMenu(fileName = "New Item Data", menuName = "SGGames/Data/Item")]
    public class ItemData : ScriptableObject
    {
        [SerializeField] private Global.ItemID m_itemID;
        
        public virtual void Picked(int amount)
        {
            
        }
    }
}

