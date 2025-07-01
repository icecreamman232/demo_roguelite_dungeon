using SGGames.Script.Core;
using UnityEngine;

namespace SGGames.Script.Data
{
    [CreateAssetMenu(menuName = "SGGames/Data/Room", fileName = "New Room Data")]
    public class RoomData : ScriptableObject
    {
        [SerializeField] private Global.RoomType m_roomType;
        [SerializeField] private GameObject m_roomPrefab;
        
        public Global.RoomType RoomType => m_roomType;
        public GameObject RoomPrefab => m_roomPrefab;
    }
}

