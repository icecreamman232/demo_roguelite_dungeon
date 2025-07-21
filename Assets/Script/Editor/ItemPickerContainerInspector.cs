using SGGames.Script.Data;
using SGGames.Script.Pickables;
using UnityEditor;
using UnityEngine;


namespace SGGames.Script.EditorExtensions
{
    [CustomEditor(typeof(ItemPickerContainer))]
    public class ItemPickerContainerInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            if (GUILayout.Button("Find Pickable"))
            {
                FindPickable();
            }
        }

        private void FindPickable()
        {
            ((ItemPickerContainer)target).ClearAllContainer();
            
            var allGUIDs = AssetDatabase.FindAssets("t:Prefab");
            foreach (var guid in allGUIDs)
            {
                var prefabAsset = AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(guid));
                var component = prefabAsset.GetComponent<Pickables.ItemPicker>();
                if (component != null)
                {
                    ((ItemPickerContainer)target).AddPickable(prefabAsset,component.ItemData.ItemID);
                    
                    if (component is AutoItemPicker)
                    {
                        ((ItemPickerContainer)target).AddAutoPickable(prefabAsset);
                    }
                }
                
            }
            EditorUtility.SetDirty((ItemPickerContainer)target);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}
