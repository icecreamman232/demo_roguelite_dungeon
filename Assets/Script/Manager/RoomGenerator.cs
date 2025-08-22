using System.Collections.Generic;
using SGGames.Scripts.Core;
using SGGames.Scripts.Data;
using SGGames.Scripts.Managers;
using UnityEngine;

namespace SGGames.Script.Managers
{
    public class RoomGenerator
    {
        private readonly RoomManager m_roomManager;
        
        private List<RoomData> GetEasyRoomList => m_roomManager.GetCurrentRoomContainer().GetEasyRoomList;
        private List<RoomData> GetHardRoomList => m_roomManager.GetCurrentRoomContainer().GetHardRoomList;
        private List<RoomData> GetChallengeRoomList => m_roomManager.GetCurrentRoomContainer().GetChallengeRoomList;
        
        public RoomGenerator(RoomManager roomManager)
        {
            m_roomManager = roomManager;
        }
        
        public void GenerateRoomForCurrentBiomes()
        {
            m_roomManager.ClearData();
            
            // Get shuffled room lists by difficulty
            var shuffledEasyRooms = GetShuffledRoomList(GetEasyRoomList);
            var shuffledHardRooms = GetShuffledRoomList(GetHardRoomList);
            var shuffledChallengeRooms = GetShuffledRoomList(GetChallengeRoomList);
            
            // Calculate how many rooms to select
            int easyRoomsToSelect = CalculateRoomsToSelect(m_roomManager.MaxEasyRoom, shuffledEasyRooms.Count);
            int hardRoomsToSelect = CalculateRoomsToSelect(m_roomManager.MaxHardRoom, shuffledHardRooms.Count);
            int challengeRoomsToSelect = CalculateRoomsToSelect(m_roomManager.MaxChallengeRoom, shuffledChallengeRooms.Count);
            
            // Distribute rooms to left and right lists
            DistributeRooms(shuffledEasyRooms, easyRoomsToSelect);
            DistributeRooms(shuffledHardRooms, hardRoomsToSelect);
            DistributeRooms(shuffledChallengeRooms, challengeRoomsToSelect);
        }
        
        private List<RoomData> GetShuffledRoomList(List<RoomData> originalList)
        {
            var shuffledList = new List<RoomData>();
            shuffledList.AddRange(originalList);
            GameHelper.Shuffle(shuffledList);
            return shuffledList;
        }
        
        private int CalculateRoomsToSelect(int maxRoomsMultiplier, int availableRoomsCount)
        {
            return Mathf.Min(maxRoomsMultiplier * 2, availableRoomsCount);
        }
        
        private void DistributeRooms(List<RoomData> roomList, int roomsToSelect)
        {
            int halfPoint = roomsToSelect / 2;
            
            // Add first half to left room list
            for (int i = 0; i < halfPoint; i++)
            {
                m_roomManager.LeftRoomList.Add(roomList[i]);
            }
            
            // Add second half to right room list
            for (int i = halfPoint; i < roomsToSelect; i++)
            {
                m_roomManager.RightRoomList.Add(roomList[i]);
            }
        }
    }
}