using SGGames.Script.Core;
using SGGames.Script.Data;
using UnityEditor;
using UnityEngine;


namespace SGGames.Script.EditorExtensions
{
    [CustomEditor(typeof(RoomContainer))]
    public class RoomContainerInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            

            if (GUILayout.Button("Find All & Save"))
            {
                ((RoomContainer)target).ClearContainer();
                
                var allGUIDs = AssetDatabase.FindAssets("t:RoomData");
                foreach (var guid in allGUIDs)
                {
                    var data = AssetDatabase.LoadAssetAtPath<RoomData>(AssetDatabase.GUIDToAssetPath(guid));
                    ((RoomContainer)target).AddItem(data);
                }
                
                EditorUtility.SetDirty((RoomContainer)target);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
            
            if (GUILayout.Button("Sort"))
            {
                var roomContainer = (RoomContainer)target;
                roomContainer.ClearSubContainer();
                
                foreach (var data in roomContainer.GetContainer)
                {
                    switch (data.RoomType)
                    {
                        case Global.RoomType.Normal:
                            roomContainer.AddNormalRoom(data);
                            break;
                        case Global.RoomType.Boss:
                            roomContainer.AddBossRoom(data);
                            break;
                        case Global.RoomType.NPC_WeaponShop:
                            roomContainer.AddNPCWeaponShopRoom(data);
                            break;
                        case Global.RoomType.NPC_ItemShop:
                            roomContainer.AddNPCItemShopRoom(data);
                            break;
                        case Global.RoomType.MiniBoss:
                            roomContainer.AddMiniBossRoom(data);
                            break;
                    }
                }
                
                EditorUtility.SetDirty((RoomContainer)target);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }
    }
}
