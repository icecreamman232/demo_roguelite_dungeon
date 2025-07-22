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
            ((ItemPickerContainer)target).ClearContainer();
            var allGUIDs = AssetDatabase.FindAssets("t:Prefab");
            
            foreach (var guid in allGUIDs)
            {
                var prefabAsset = AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(guid));
                var component = prefabAsset.GetComponent<Pickables.ItemPicker>();
                if (component != null && component.ItemData != null)
                {
                    ((ItemPickerContainer)target).AddItemPicker(component);
                }
            }
            
            
            EditorUtility.SetDirty((ItemPickerContainer)target);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}
