using SGGames.Scripts.Data;
using SGGames.Scripts.Pickables;
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
            
            if (GUILayout.Button("Find All"))
            {
                FindPickable();
                FindCurrency();
            }
            
            if (GUILayout.Button("Find Item"))
            {
                FindPickable();
            }
            
            if (GUILayout.Button("Find Currency"))
            {
                FindCurrency();
            }
        }

        private void SaveData()
        {
            EditorUtility.SetDirty((ItemPickerContainer)target);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private void ClearCurrencyData()
        {
            ((ItemPickerContainer)target).ClearCurrencyData();
        }

        private void ClearItemData()
        {
            ((ItemPickerContainer)target).ClearItemData();
        }

        private void FindPickable()
        {
            ClearItemData();
            var allGUIDs = AssetDatabase.FindAssets("t:Prefab");
            
            foreach (var guid in allGUIDs)
            {
                var prefabAsset = AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(guid));
                var component = prefabAsset.GetComponent<ManualItemPicker>();
                if (component != null && component.ItemData != null)
                {
                    ((ItemPickerContainer)target).AddManualItemPicker(component);
                }
            }

            SaveData();

        }
        
        private void FindCurrency()
        {
            ClearCurrencyData();
            var allGUIDs = AssetDatabase.FindAssets("t:Prefab");
            
            foreach (var guid in allGUIDs)
            {
                var prefabAsset = AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(guid));
                var component = prefabAsset.GetComponent<CurrencyPicker>();
                if (component != null)
                {
                    ((ItemPickerContainer)target).AddCurrencyPicker(component);
                }
            }

            SaveData();
        }
    }
}
