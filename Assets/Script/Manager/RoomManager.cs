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

        private Global.RoomRewardType m_currentRoomReward;
        private RoomRewardGenerator m_roomRewardGenerator;
        private RoomGenerator m_roomGenerator;
        
        private void Awake()
        {
            ServiceLocator.RegisterService<RoomManager>(this);
            m_leftRoomList = new List<RoomData>();
            m_rightRoomList = new List<RoomData>();
            m_roomRewardGenerator = new RoomRewardGenerator();
            m_roomGenerator = new RoomGenerator(this);
        }
        
        public void GenerateRoomRewardForCurrentBiomes()
        {
            m_leftRoomRewardList.Clear();
            m_rightRoomRewardList.Clear();
            
            m_roomRewardGenerator.GenerateRoomReward(m_maxRoom);
            m_leftRoomRewardList.AddRange(m_roomRewardGenerator.ResultList);
            
            m_roomRewardGenerator.GenerateRoomReward(m_maxRoom);
            m_rightRoomRewardList.AddRange(m_roomRewardGenerator.ResultList);
        }

        public void ClearData()
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
            
            if (m_currentRoomIndex == m_maxRoom - 1)
            {
                //TODO:Load default first boss. THis should be another random to choose between bosses for a biome
                return m_roomContainers[m_currentBiomesIndex].GetBossRoomList[0];
            }
            
            if (m_currentRoomIndex >= m_maxRoom)
            {
                m_currentRoomIndex = 0;
            }

            m_currentRoomReward = GetLeftRoomReward();
            
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
            
            m_currentRoomReward = GetRightRoomReward();

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
        
        public Global.RoomRewardType CurrentRoomReward => m_currentRoomReward;
        public bool IsBossRoom => m_currentRoomIndex == m_maxRoom - 1;
        public RoomContainer GetCurrentRoomContainer() => m_roomContainers[m_currentBiomesIndex];
        public int MaxEasyRoom => m_maxEasyRoom;
        public int MaxHardRoom => m_maxHardRoom;
        public int MaxChallengeRoom => m_maxChallengeRoom;

        public int CurrentBiomesIndex => m_currentBiomesIndex;
        
        public RoomData FirstRoom => m_roomContainers[m_currentBiomesIndex].FirstRoom;
        
        public RoomGenerator RoomGenerator
        {
            get { return m_roomGenerator; }
        }

        public List<RoomData> LeftRoomList
        {
            get => m_leftRoomList;
            set => m_leftRoomList = value;
        }

        public List<RoomData> RightRoomList
        {
            get => m_rightRoomList;
            set => m_rightRoomList = value;
        }
    }
}
