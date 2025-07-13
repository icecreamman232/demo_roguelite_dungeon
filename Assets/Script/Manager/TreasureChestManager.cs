using SGGames.Script.Core;
using UnityEngine;

namespace SGGames.Script.Managers
{
    public class TreasureChestManager : MonoBehaviour, IGameService
    {
        [SerializeField] private GameObject m_treasureChestPrefab;
        
        private void Awake()
        {
            ServiceLocator.RegisterService<TreasureChestManager>(this);
        }

        public GameObject GetTreasureChestWith(int biome)
        {
            //TODO:This is for temporary spawning the chest. It should be based on the biome value to choose the proper chest
            return m_treasureChestPrefab;
            //return null;
        }
        
        public GameObject GetTreasureChestWith(Global.RoomType roomType)
        {
            return null;
        }
    }
}

