using System.Collections.Generic;
using SGGames.Script.Core;
using SGGames.Script.Events;
using SGGames.Scripts.Entity;
using UnityEngine;

namespace SGGames.Script.Dungeon
{
    public class Room : MonoBehaviour
    {
        [SerializeField] private GameEvent m_gameEvent;
        private List<EnemyController> m_enemyList;
        private int m_totalEnemyAlive;
        
        
        private void Awake()
        {
            m_enemyList = new List<EnemyController>();
            m_totalEnemyAlive = 0;
            m_gameEvent.AddListener(OnReceiveGameEvent);
        }
        
        private void OnReceiveGameEvent(Global.GameEventType eventType)
        {
            if (eventType == Global.GameEventType.LoadNextRoom)
            {
                m_gameEvent.RemoveListener(OnReceiveGameEvent);
                Destroy(this.gameObject);
            }
        }

        public void RegisterEnemyToRoom(EnemyController enemyController)
        {
            m_enemyList.Add(enemyController);
            m_totalEnemyAlive++;
        }
        
        public void OnEnemyDeath()
        {
            m_totalEnemyAlive--;
            if (m_totalEnemyAlive <= 0)
            {
                m_gameEvent.Raise(Global.GameEventType.RoomCleared);
            }
        }
    }
}
