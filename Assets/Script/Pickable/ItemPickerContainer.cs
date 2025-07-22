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
        
        public GameObject GetPrefabWithID(Global.ItemID id)
        {
            var item = m_itemPickers.FirstOrDefault(item =>item.ItemData.ItemID == id);
            if (item == null) return null;
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
