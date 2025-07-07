using SGGames.Script.Data;
using SGGames.Script.Pickables;
using UnityEditor;
using UnityEngine;


namespace SGGames.Script.EditorExtensions
{
    [CustomEditor(typeof(PickableContainer))]
    public class PickableContainerInspector : Editor
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
            ((PickableContainer)target).ClearAllContainer();
            
            var allGUIDs = AssetDatabase.FindAssets("t:Prefab");
            foreach (var guid in allGUIDs)
            {
                var prefabAsset = AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(guid));
                var component = prefabAsset.GetComponent<Pickables.Pickable>();
                if (component != null)
                {
                    ((PickableContainer)target).AddPickable(prefabAsset,component.ItemData.ItemID);
                    
                    if (component is AutoPickable)
                    {
                        ((PickableContainer)target).AddAutoPickable(prefabAsset);
                    }
                }
                
            }
            EditorUtility.SetDirty((PickableContainer)target);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}
