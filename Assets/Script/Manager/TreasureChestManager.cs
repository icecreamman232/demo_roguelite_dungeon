using SGGames.Script.Core;
using SGGames.Script.Events;
using UnityEngine;

namespace SGGames.Script.Managers
{
    public class TreasureChestManager : MonoBehaviour, IGameService
    {
        [SerializeField] private SpawnChestEvent m_spawnChestEvent;
        [SerializeField] private GameObject m_treasureChestPrefab;
        
        private void Awake()
        {
            ServiceLocator.RegisterService<TreasureChestManager>(this);
            m_spawnChestEvent.AddListener(OnReceiveSpawnChestEvent);
        }
        
        private void OnReceiveSpawnChestEvent(Vector3 spawnPosition)
        {
            var treasureChestPrefab = GetTreasureChestWith(-1);
            Instantiate(treasureChestPrefab, spawnPosition,Quaternion.identity);
        }

        private GameObject GetTreasureChestWith(int biome)
        {
            //TODO:This is for temporary spawning the chest. It should be based on the biome value to choose the proper chest
            return m_treasureChestPrefab;
            //return null;
        }
        
        public GameObject GetTreasureChestWith(Global.RoomType roomType)
        {
            return null;
        }
        
        // var treasureChestManager = ServiceLocator.GetService<TreasureChestManager>();
        // var treasureChestPrefab = treasureChestManager.GetTreasureChestWith(-1);
        // Instantiate(treasureChestPrefab, m_treasureChestSpawnPoint);
    }
}

