using System.Collections.Generic;
using System.Linq;
using SGGames.Script.Core;
using SGGames.Script.Pickables;
using UnityEngine;

namespace SGGames.Script.Data
{
    [CreateAssetMenu(fileName = "Item Picker Container", menuName = "SGGames/Data/Item Picker Container")]
    public class ItemPickerContainer : ScriptableObject
    {
        [SerializeField] private List<ItemPicker> m_itemPickers;
        
        private Dictionary<Global.ItemID, GameObject> m_dictionary = new Dictionary<Global.ItemID, GameObject>();
        
        public GameObject GetPrefabWithID(Global.ItemID id)
        {
            var item = m_itemPickers.First(item =>item.ItemData.ItemID == id);
            return item.gameObject;
        }

        public void AddItemPicker(ItemPicker itemPicker)
        {
            m_itemPickers.Add(itemPicker);
        }
        
        public void ClearContainer()
        {
            m_itemPickers.Clear();
        }
    }
}
