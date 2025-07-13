using UnityEngine;

namespace SGGames.Script.Data
{
    [CreateAssetMenu(fileName = "Asset Path Container", menuName = "SGGames/Asset Path Container")]
    public class AssetPathContainer : ScriptableObject
    {
        [SerializeField] private string m_enemyHealthBarPrefabPath;
        [SerializeField] private string m_fillColorSpritePrefabPath;
        [SerializeField] private string m_lootTablePrefabPath;

        public string EnemyHPBarPath
        {
            get => m_enemyHealthBarPrefabPath;
            set => m_enemyHealthBarPrefabPath = value;
        }
        public string FillColorSpritePath
        {
            get => m_fillColorSpritePrefabPath;
            set => m_fillColorSpritePrefabPath = value;
        }
        public string LootTablePath
        {
            get => m_lootTablePrefabPath;
            set => m_lootTablePrefabPath = value;
        }
        
    }
}
