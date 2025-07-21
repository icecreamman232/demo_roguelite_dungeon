using UnityEngine;

namespace SGGames.Script.Data
{
    [CreateAssetMenu(fileName = "Item Rarity Table", menuName = "SGGames/Data/Item Rarity Table")]
    public class ItemRarityTable : ScriptableObject
    {
        [SerializeField] private WeightSheet[] m_itemRaritySheet;
        
        [ContextMenu("Calculate Percentage")]
        private void CalculateAllPercentage()
        {
            foreach (var sheet in m_itemRaritySheet)
            {
                sheet.CalculatePercentage();
            }
        }
        
        public WeightSheet GetItemRaritySheet(int biomesIndex) => m_itemRaritySheet[biomesIndex];
    }
}
