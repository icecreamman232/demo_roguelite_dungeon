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
        [SerializeField] private List<CurrencyPicker> m_currencyPickers;
        [SerializeField] private List<ManualItemPicker> m_itemPickers;
        
        public GameObject GetCurrencyPrefabWithID(Global.ItemID id)
        {
            var item = m_currencyPickers.FirstOrDefault(item =>item.ItemID == id);
            if (item == null) return null;
            return item.gameObject;
        }
        
        public GameObject GetItemPrefabWithID(Global.ItemID id)
        {
            var item = m_itemPickers.FirstOrDefault(item =>item.ItemData.ItemID == id);
            if (item == null) return null;
            return item.gameObject;
        }

        public void AddManualItemPicker(ManualItemPicker itemPicker)
        {
            m_itemPickers.Add(itemPicker);
        }

        public void AddCurrencyPicker(CurrencyPicker currencyPicker)
        {
            m_currencyPickers.Add(currencyPicker);       
        }

        public void ClearItemData()
        {
            m_itemPickers.Clear();
        }

        public void ClearCurrencyData()
        {
            m_currencyPickers.Clear();
        }
    }
}
