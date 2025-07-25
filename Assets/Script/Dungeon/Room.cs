using System.Collections.Generic;
using SGGames.Script.Core;
using SGGames.Script.Events;
using SGGames.Script.PathFindings;
using SGGames.Scripts.Entity;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace SGGames.Script.Dungeon
{
    public class Room : MonoBehaviour
    {
        [SerializeField] private GameEvent m_gameEvent;
        [SerializeField] private SpawnChestEvent m_spawnChestEvent;
        [SerializeField] private Transform m_treasureChestSpawnPoint;
        [SerializeField] private Tilemap m_tilemap;
        
        private List<EnemyController> m_enemyList;
        private int m_totalEnemyAlive;
        
        
        private void Awake()
        {
            m_enemyList = new List<EnemyController>();
            m_totalEnemyAlive = 0;
            m_gameEvent.AddListener(OnReceiveGameEvent);
            var gridManager = ServiceLocator.GetService<GridManager>();
            gridManager.Tilemap = m_tilemap;
        }
        
        private void OnReceiveGameEvent(Global.GameEventType eventType)
        {
            if (eventType == Global.GameEventType.LoadNextRoomLeftRoom || eventType == Global.GameEventType.LoadNextRoomRightRoom)
            {
                m_gameEvent.RemoveListener(OnReceiveGameEvent);
                Destroy(this.gameObject);
            }
        }
        
        public void KillAllEnemiesInRoom()
        {
            //Debug.Log("Room Clear");
            for (int i = 0; i < m_enemyList.Count; i++)
            {
                m_enemyList[i].Kill();
            }
        }

        private void RoomClearAndDropChest()
        {
            m_gameEvent.Raise(Global.GameEventType.RoomCleared);
            m_spawnChestEvent.Raise(m_treasureChestSpawnPoint.position);
        }

        public void RegisterEnemyToRoom(EnemyController enemyController)
        {
            m_enemyList.Add(enemyController);
            m_totalEnemyAlive++;
        }

        public bool HasThisEnemyRegistered(EnemyController enemyController)
        {
            return m_enemyList.Contains(enemyController);
        }
        
        public void OnEnemyDeath()
        {
            m_totalEnemyAlive--;
            if (m_totalEnemyAlive <= 0)
            {
                RoomClearAndDropChest();
            }
        }
    }
}
