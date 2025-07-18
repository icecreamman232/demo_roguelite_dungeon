using System.Collections.Generic;
using SGGames.Script.Core;
using SGGames.Script.Data;
using SGGames.Script.Dungeon;
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
        [Header("Room Layout")]
        [SerializeField] private RoomContainer[] m_roomContainers;
        [SerializeField] private RoomContainer m_roomContainer;
        [SerializeField] private List<RoomData> m_leftRoomList;
        [SerializeField] private List<RoomData> m_rightRoomList;
        [Header("Room Reward")]
        [SerializeField] private List<Global.RoomRewardType> m_leftRoomRewardList;
        [SerializeField] private List<Global.RoomRewardType> m_rightRoomRewardList;

        private RoomRewardGenerator m_roomRewardGenerator;
        
        public int CurrentBiomesIndex => m_currentBiomesIndex;
        
        private void Awake()
        {
            ServiceLocator.RegisterService<RoomManager>(this);
            m_leftRoomList = new List<RoomData>();
            m_rightRoomList = new List<RoomData>();
            m_roomRewardGenerator = new RoomRewardGenerator();
        }
        
        public void GenerateRoomForCurrentBiomes()
        {
            ClearData();
            
            var shuffledEasyRoomList = new List<RoomData>();
            shuffledEasyRoomList.AddRange(m_roomContainers[m_currentBiomesIndex].GetEasyRoomList);
            GameHelper.Shuffle(shuffledEasyRoomList);
            
            var shuffledHardRoomList = new List<RoomData>();
            shuffledHardRoomList.AddRange(m_roomContainers[m_currentBiomesIndex].GetHardRoomList);
            GameHelper.Shuffle(shuffledHardRoomList);
            
            var shuffledChallengeRoomList = new List<RoomData>();
            shuffledChallengeRoomList.AddRange(m_roomContainers[m_currentBiomesIndex].GetChallengeRoomList);
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

        [ContextMenu("Test Reward Generation")]
        private void TestRewardGeneration()
        {
            m_roomRewardGenerator.GenerateRoomReward(9);

            for (int i = 0; i < m_roomRewardGenerator.ResultList.Count; i++)
            {
                Debug.Log($"Reward Room {i} : {m_roomRewardGenerator.ResultList[i].ToString()}");
            }
        }
        
        public void GenerateRoomRewardForCurrentBiomes()
        {
            m_leftRoomRewardList.Clear();
            m_rightRoomRewardList.Clear();
            
            m_roomRewardGenerator.GenerateRoomReward(9);
            m_leftRoomRewardList.AddRange(m_roomRewardGenerator.ResultList);
            
            m_roomRewardGenerator.GenerateRoomReward(9);
            m_rightRoomRewardList.AddRange(m_roomRewardGenerator.ResultList);
        }
        
        private void ClearData()
        {
            m_currentRoomIndex = 0;
            m_leftRoomList.Clear();
            m_rightRoomList.Clear();
            m_leftRoomRewardList.Clear();
            m_rightRoomRewardList.Clear();
        }

        public Global.RoomRewardType GetLeftRoomReward()
        {
            return m_leftRoomRewardList[m_currentRoomIndex];
        }

        public Global.RoomRewardType GetRightRoomReward()
        {
            return m_rightRoomRewardList[m_currentRoomIndex];
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

        public void IncreaseBiomeIndex()
        {
            m_currentBiomesIndex++;
        }
        
        public bool IsBossRoom => m_currentRoomIndex == m_maxRoom - 1;
    }
}
