using SGGames.Script.Core;
using UnityEngine;

namespace SGGames.Script.Data
{
    [CreateAssetMenu(menuName = "SGGames/Data/Room", fileName = "New Room Data")]
    public class RoomData : ScriptableObject
    {
        [SerializeField] private Global.BiomesName m_biomesName;
        [SerializeField] private Global.RoomType m_roomType;
        [SerializeField] private Global.RoomDifficulty m_roomDifficulty;
        [SerializeField] private GameObject m_roomPrefab;
        
        public Global.BiomesName BiomesName => m_biomesName;
        public Global.RoomType RoomType => m_roomType;
        public Global.RoomDifficulty RoomDifficulty => m_roomDifficulty;
        public GameObject RoomPrefab => m_roomPrefab;
    }
}

