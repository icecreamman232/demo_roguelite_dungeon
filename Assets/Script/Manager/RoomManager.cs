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
        [SerializeField] private int m_maxEasyRoom;
        [SerializeField] private int m_maxHardRoom;
        [SerializeField] private int m_maxChallengeRoom;
        [SerializeField] private int m_maxSpecialRoom; //Special room includes: npc room, mini boss room, all except normal room
        [Header("Data")] 
        [SerializeField] private int m_currentBiomesIndex;
        [SerializeField] private int m_currentRoomIndex;
        [SerializeField] private RoomContainer[] m_roomContainers;
        [SerializeField] private RoomContainer m_roomContainer;
        [SerializeField] private List<RoomData> m_leftRoomList;
        [SerializeField] private List<RoomData> m_rightRoomList;
        
        private void Awake()
        {
            ServiceLocator.RegisterService<RoomManager>(this);
            m_leftRoomList = new List<RoomData>();
            m_rightRoomList = new List<RoomData>();
        }

        private void Start()
        {
            GenerateRoomForBiomes(m_currentBiomesIndex);
        }

        private void GenerateRoomForBiomes(int biomesIndex)
        {
            ClearData();
            
            var shuffledEasyRoomList = new List<RoomData>();
            shuffledEasyRoomList.AddRange(m_roomContainers[biomesIndex].GetEasyRoomList);
            GameHelper.Shuffle(shuffledEasyRoomList);
            
            var shuffledHardRoomList = new List<RoomData>();
            shuffledHardRoomList.AddRange(m_roomContainers[biomesIndex].GetHardRoomList);
            GameHelper.Shuffle(shuffledHardRoomList);
            
            var shuffledChallengeRoomList = new List<RoomData>();
            shuffledChallengeRoomList.AddRange(m_roomContainers[biomesIndex].GetChallengeRoomList);
            GameHelper.Shuffle(shuffledChallengeRoomList);
            
            
            int numberEasyRoomToSelect = Mathf.Min(m_maxEasyRoom * 2, shuffledEasyRoomList.Count);
            int numberHardRoomToSelect = Mathf.Min(m_maxHardRoom * 2, shuffledHardRoomList.Count);
            int numberChallengeRoomToSelect = Mathf.Min(m_maxChallengeRoom * 2, shuffledChallengeRoomList.Count);
            
            //Debug.Log($"RoomManager::Pick Easy:{numberEasyRoomToSelect} Hard:{numberHardRoomToSelect} Challenge:{numberChallengeRoomToSelect}");
            
            //Easy
            for (int e = 0; e < numberEasyRoomToSelect/2; e++)
            {
                m_leftRoomList.Add(shuffledEasyRoomList[e]);
            }
            
            for (int e = numberEasyRoomToSelect/2; e < numberEasyRoomToSelect; e++)
            {
                m_rightRoomList.Add(shuffledEasyRoomList[e]);
            }
            
            //Hard
            for (int h = 0; h < numberHardRoomToSelect/2; h++)
            {
                m_leftRoomList.Add(shuffledHardRoomList[h]);
            }
            
            for (int h = numberHardRoomToSelect/2; h < numberHardRoomToSelect; h++)
            {
                m_rightRoomList.Add(shuffledHardRoomList[h]);
            }
            
            //Challenge
            for (int c = 0; c < numberChallengeRoomToSelect/2; c++)
            {
                m_leftRoomList.Add(shuffledChallengeRoomList[c]);
            }
            
            for (int c = numberChallengeRoomToSelect/2; c < numberChallengeRoomToSelect; c++)
            {
                m_rightRoomList.Add(shuffledChallengeRoomList[c]);
            }
        }

        [ContextMenu("Generate Rooms")]
        private void GenerateRooms()
        {
            ClearData();
            
            var shuffledEasyRoomList = new List<RoomData>();
            shuffledEasyRoomList.AddRange(m_roomContainer.GetEasyRoomList);
            GameHelper.Shuffle(shuffledEasyRoomList);
            
            var shuffledHardRoomList = new List<RoomData>();
            shuffledHardRoomList.AddRange(m_roomContainer.GetHardRoomList);
            GameHelper.Shuffle(shuffledHardRoomList);
            
            var shuffledChallengeRoomList = new List<RoomData>();
            shuffledChallengeRoomList.AddRange(m_roomContainer.GetChallengeRoomList);
            GameHelper.Shuffle(shuffledChallengeRoomList);
            
            
            int numberEasyRoomToSelect = Mathf.Min(m_maxEasyRoom * 2, shuffledEasyRoomList.Count);
            int numberHardRoomToSelect = Mathf.Min(m_maxHardRoom * 2, shuffledHardRoomList.Count);
            int numberChallengeRoomToSelect = Mathf.Min(m_maxChallengeRoom * 2, shuffledChallengeRoomList.Count);
            
            //Debug.Log($"RoomManager::Pick Easy:{numberEasyRoomToSelect} Hard:{numberHardRoomToSelect} Challenge:{numberChallengeRoomToSelect}");
            
            //Easy
            for (int e = 0; e < numberEasyRoomToSelect/2; e++)
            {
                m_leftRoomList.Add(shuffledEasyRoomList[e]);
            }
            
            for (int e = numberEasyRoomToSelect/2; e < numberEasyRoomToSelect; e++)
            {
                m_rightRoomList.Add(shuffledEasyRoomList[e]);
            }
            
            //Hard
            for (int h = 0; h < numberHardRoomToSelect/2; h++)
            {
                m_leftRoomList.Add(shuffledHardRoomList[h]);
            }
            
            for (int h = numberHardRoomToSelect/2; h < numberHardRoomToSelect; h++)
            {
                m_rightRoomList.Add(shuffledHardRoomList[h]);
            }
            
            //Challenge
            for (int c = 0; c < numberChallengeRoomToSelect/2; c++)
            {
                m_leftRoomList.Add(shuffledChallengeRoomList[c]);
            }
            
            for (int c = numberChallengeRoomToSelect/2; c < numberChallengeRoomToSelect; c++)
            {
                m_rightRoomList.Add(shuffledChallengeRoomList[c]);
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
            if (m_currentRoomIndex == m_maxRoom - 1)
            {
                //TODO:Load default first boss. THis should be another random to choose between bosses for a biome
                return m_roomContainers[m_currentBiomesIndex].GetBossRoomList[0];
            }

            if (m_currentRoomIndex == m_maxRoom)
            {
                m_currentRoomIndex = 0;
            }
            return m_rightRoomList[m_currentRoomIndex];
        }
        
        public bool IsBossRoom => m_currentRoomIndex == m_maxRoom - 1;
    }
}
