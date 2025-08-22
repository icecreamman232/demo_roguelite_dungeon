using System.Collections.Generic;
using SGGames.Scripts.Core;
using SGGames.Scripts.Data;
using UnityEngine;

namespace SGGames.Scripts.Managers
{
    public class ItemLibrary : MonoBehaviour, IGameService
    {
        [SerializeField] private InventoryItemData[] m_itemData;
        [SerializeField] private List<InventoryItemData> m_commonList;
        [SerializeField] private List<InventoryItemData> m_uncommonList;
        [SerializeField] private List<InventoryItemData> m_rareList;
        [SerializeField] private List<InventoryItemData> m_legendaryList;
        
        private void Awake()
        {
            ServiceLocator.RegisterService<ItemLibrary>(this);
            FillItemIntoTheirCategory();
        }
        
        private void FillItemIntoTheirCategory()
        {
            FindCommon();
            FindUncommon();
            FindRare();
            FindLegendary();
        }

        [ContextMenu("Find Common")]
        private void FindCommon()
        {
           m_commonList.AddRange(FindItem(Global.ItemRarity.Common));
        }
        
        [ContextMenu("Find Uncommon")]
        private void FindUncommon()
        {
           m_uncommonList.AddRange(FindItem(Global.ItemRarity.Common));
        }
        
        [ContextMenu("Find Rare")]
        private void FindRare()
        {
            m_rareList.AddRange(FindItem( Global.ItemRarity.Common));
        }
        
        [ContextMenu("Find Legendary")]
        private void FindLegendary()
        {
            m_legendaryList.AddRange(FindItem(Global.ItemRarity.Common));
        }
        
        private List<InventoryItemData> FindItem(Global.ItemRarity rarity)
        {
            var itemList = new List<InventoryItemData>();
            for (int i = 0; i < m_itemData.Length; i++)
            {
                if (m_itemData[i].Rarity == rarity)
                {
                    itemList.Add(m_itemData[i]);
                }
            }
            return itemList;
        }
        
        public InventoryItemData GetCommonItem()
        {
            if (m_commonList.Count == 0) return null;
            var randomIndex = Random.Range(0, m_commonList.Count);
            var item = m_commonList[randomIndex];
            m_commonList.RemoveAt(randomIndex);
            return item;
        }

        public InventoryItemData GetUncommonItem()
        {
            if (m_uncommonList.Count == 0) return null;
            var randomIndex = Random.Range(0, m_commonList.Count);
            var item = m_uncommonList[randomIndex];
            m_uncommonList.RemoveAt(randomIndex);
            return item;
        }
        
        public InventoryItemData GetRareItem()
        {
            if (m_rareList.Count == 0) return null;
            var randomIndex = Random.Range(0, m_commonList.Count);
            var item = m_rareList[randomIndex];
            m_rareList.RemoveAt(randomIndex);
            return item;
        }

        public InventoryItemData GetLegendaryItem()
        {
            if (m_legendaryList.Count == 0) return null;
            var randomIndex = Random.Range(0, m_commonList.Count);
            var item = m_legendaryList[randomIndex];
            m_legendaryList.RemoveAt(randomIndex);
            return item;
        }
    }
}
