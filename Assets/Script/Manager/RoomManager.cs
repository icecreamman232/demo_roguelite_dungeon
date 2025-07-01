using System.Collections.Generic;
using SGGames.Script.Core;
using SGGames.Script.Data;
using UnityEngine;

namespace SGGames.Script.Managers
{
    public class RoomManager : MonoBehaviour, IGameService
    {
        [Header("Floor Parameters")]
        [SerializeField] private int m_maxRoom; //Max number of room, not included boss room
        [SerializeField] private int m_maxSpecialRoom; //Special room includes: npc room, mini boss room, all except normal room
        [Header("Data")] 
        [SerializeField] private int m_currentRoomIndex;
        [SerializeField] private RoomContainer m_roomContainer;
        [SerializeField] private List<RoomData> m_leftRoomList;
        [SerializeField] private List<RoomData> m_rightRoomList;
        
        private void Awake()
        {
            ServiceLocator.RegisterService<RoomManager>(this);
            m_leftRoomList = new List<RoomData>();
            m_rightRoomList = new List<RoomData>();
        }
        
        [ContextMenu("Generate Rooms")]
        private void GenerateRooms()
        {
            ClearData();
            
            var shuffledRoomList = new List<RoomData>();
            shuffledRoomList.AddRange(m_roomContainer.GetNormalRoomList);
            GameHelper.Shuffle(shuffledRoomList);
            int numberItemToSelect = Mathf.Min(m_maxRoom * 2, shuffledRoomList.Count);

            for (int i = 0; i < numberItemToSelect/2; i++)
            {
                m_leftRoomList.Add(shuffledRoomList[i]);
            }
            
            for (int i = numberItemToSelect/2; i < numberItemToSelect; i++)
            {
                m_rightRoomList.Add(shuffledRoomList[i]);
            }
            
        }

        private void ClearData()
        {
            m_currentRoomIndex = 0;
            m_leftRoomList.Clear();
            m_rightRoomList.Clear();
        }

        public RoomData GetNextLeftRoom()
        {
            m_currentRoomIndex++;
            if (m_currentRoomIndex >= m_maxRoom)
            {
                m_currentRoomIndex = 0;
            }
            return m_leftRoomList[m_currentRoomIndex];
        }

        public RoomData GetNextRightRoom()
        {
            m_currentRoomIndex++;
            if (m_currentRoomIndex >= m_maxRoom)
            {
                m_currentRoomIndex = 0;
            }
            return m_rightRoomList[m_currentRoomIndex];
        }
    }
}
