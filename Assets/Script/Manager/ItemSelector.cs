using SGGames.Script.Core;
using SGGames.Script.Data;
using Random = UnityEngine.Random;

namespace SGGames.Script.Managers
{
    /// <summary>
    /// Class to hold the logic to choose which item should be put into the chest
    /// </summary>
    public class ItemSelector
    {
        private ItemRarityTable m_itemRarityTable;
        private RoomManager m_roomManager;
        private ItemLibrary m_itemLibrary;
        
        public ItemSelector(ItemLibrary itemLibrary, RoomManager roomManager, ItemRarityTable itemRarityTable)
        {
            m_itemLibrary = itemLibrary;
            m_roomManager = roomManager;
            m_itemRarityTable = itemRarityTable;
        }

        private Global.ItemRarity GetRarity(WeightSheet weightSheet)
        {
            var randomPercentage = Random.Range(0f, 100f);
            var weightIndex = weightSheet.GetWeightValue(randomPercentage);
            return (Global.ItemRarity)weightIndex;
        }

        public InventoryItemData GetItemForChest()
        {
            var biomesWeightSheet = m_itemRarityTable.GetItemRaritySheet(m_roomManager.CurrentBiomesIndex);
            var rarity = GetRarity(biomesWeightSheet);
            switch (rarity)
            {
                case Global.ItemRarity.Common:
                    return m_itemLibrary.GetCommonItem();
                case Global.ItemRarity.Uncommon:
                    return m_itemLibrary.GetUncommonItem();
                case Global.ItemRarity.Rare:
                    return m_itemLibrary.GetRareItem();
                case Global.ItemRarity.Legendary:
                    return m_itemLibrary.GetLegendaryItem();
            }

            return null;
        }
    }
}
