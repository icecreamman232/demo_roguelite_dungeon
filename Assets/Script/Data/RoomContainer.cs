using System.Collections.Generic;
using SGGames.Scripts.Core;
using UnityEngine;

namespace SGGames.Scripts.Data
{
    [CreateAssetMenu(fileName = "New Room Container", menuName = "SGGames/Data/Room Container")]
    public class RoomContainer : IDataContainer<RoomData>
    {
        [SerializeField] private Global.BiomesName m_biomesName;
        [SerializeField] private RoomData m_firstRoom;
        [SerializeField] private List<RoomData> m_easyRooms = new List<RoomData>();
        [SerializeField] private List<RoomData> m_hardRooms = new List<RoomData>();
        [SerializeField] private List<RoomData> m_challengeRooms = new List<RoomData>();
        [SerializeField] private List<RoomData> m_bossRooms = new List<RoomData>();
        [SerializeField] private RoomData m_npcWeaponShop;
        [SerializeField] private RoomData m_npcItemShop;
        [SerializeField] private List<RoomData> m_miniBossRooms;

        public void ClearSubContainer()
        {
            m_npcWeaponShop = null;
            m_npcItemShop = null;
            m_easyRooms.Clear();
            m_hardRooms.Clear();
            m_challengeRooms.Clear();
            m_bossRooms.Clear();
            m_miniBossRooms.Clear();
        }

        public void AddNPCWeaponShopRoom(RoomData roomData)
        {
            m_npcWeaponShop = roomData;
        }

        public void AddNPCItemShopRoom(RoomData roomData)
        {
            m_npcItemShop = roomData;
        }
        
        public void AddEasyRoom(RoomData roomData)
        {
            m_easyRooms.Add(roomData);
        }
        
        public void AddHardRoom(RoomData roomData)
        {
            m_hardRooms.Add(roomData);
        }
        
        public void AddChallengeRoom(RoomData roomData)
        {
            m_challengeRooms.Add(roomData);
        }

        public void AddMiniBossRoom(RoomData roomData)
        {
            m_miniBossRooms.Add(roomData);
        }

        public void AddBossRoom(RoomData roomData)
        {
            m_bossRooms.Add(roomData);
        }

        public RoomData FirstRoom => m_firstRoom;
        public Global.BiomesName BiomesName => m_biomesName;
        public RoomData GetNPCWeaponShopRoom => m_npcWeaponShop;
        public RoomData GetNPCItemShopRoom => m_npcWeaponShop;
        public List<RoomData> GetEasyRoomList => m_easyRooms;
        public List<RoomData> GetHardRoomList => m_hardRooms;
        public List<RoomData> GetChallengeRoomList => m_challengeRooms;
        public List<RoomData> GeMiniBossRoomList => m_miniBossRooms;
        public List<RoomData> GetBossRoomList => m_bossRooms;
    }
}

