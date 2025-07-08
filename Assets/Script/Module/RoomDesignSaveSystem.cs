#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace SGGames.Script.Modules
{
    public class RoomDesignSaveSystem : MonoBehaviour
    {
        [SerializeField] private string m_prefixName;
        [SerializeField] private int m_startIndex;
        [SerializeField] private GameObject m_designRoom;
        [SerializeField] private GameObject m_roomTemplatePrefab;

        private readonly string SAVE_PATH = "Assets/Prefab/Room/";
        
        private string GetNextRoomName()
        {
            var roomName = m_prefixName + m_startIndex + ".prefab";
            m_startIndex++;
            return roomName;
        }
        
        public void SaveLevel()
        {
            Debug.Log("Room design has been saved");
            var savePath = SAVE_PATH + GetNextRoomName();
            var savedPrefab = PrefabUtility.SaveAsPrefabAsset(m_designRoom, savePath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = savedPrefab;
        }

        public void ResetLevel()
        {
            Debug.Log("Room design has been reset");
            m_designRoom.transform.position = Vector3.zero;
            m_designRoom.transform.localRotation = Quaternion.identity;
            m_designRoom.transform.localScale = Vector3.one;
            PrefabUtility.RevertPrefabInstance(m_designRoom,InteractionMode.UserAction);
            
        }
    }
}
#endif
