using System.Collections.Generic;
using UnityEngine;

namespace SGGames.Script.Data
{
    [CreateAssetMenu(fileName = "New Room Container", menuName = "SGGames/Data/Room Container")]
    public class RoomContainer : IDataContainer<RoomData>
    {
        [SerializeField] private List<RoomData> m_normalRooms = new List<RoomData>();
        [SerializeField] private List<RoomData> m_bossRooms = new List<RoomData>();
        [SerializeField] private RoomData m_npcWeaponShop;
        [SerializeField] private RoomData m_npcItemShop;
        [SerializeField] private List<RoomData> m_miniBossRooms;

        public void ClearSubContainer()
        {
            m_npcWeaponShop = null;
            m_normalRooms.Clear();
            m_bossRooms.Clear();
        }

        public void AddNPCWeaponShopRoom(RoomData roomData)
        {
            m_npcWeaponShop = roomData;
        }

        public void AddNPCItemShopRoom(RoomData roomData)
        {
            m_npcItemShop = roomData;
        }
        
        public void AddNormalRoom(RoomData roomData)
        {
            m_normalRooms.Add(roomData);
        }

        public void AddMiniBossRoom(RoomData roomData)
        {
            m_miniBossRooms.Add(roomData);
        }

        public void AddBossRoom(RoomData roomData)
        {
            m_bossRooms.Add(roomData);
        }

        public RoomData GetNPCWeaponShopRoom => m_npcWeaponShop;
        public RoomData GetNPCItemShopRoom => m_npcWeaponShop;
        public List<RoomData> GetNormalRoomList => m_normalRooms;
        public List<RoomData> GeMiniBossRoomList => m_miniBossRooms;
        public List<RoomData> GetBossRoomList => m_bossRooms;
    }
}

