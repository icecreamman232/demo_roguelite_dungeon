using SGGames.Scripts.Managers;
using UnityEditor;
using UnityEngine;

namespace SGGames.Scripts.EditorExtensions
{
    [CustomEditor(typeof(DebugSettings))]
    public class DebugSettingInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            if(GUILayout.Button("Reset Settings", GUILayout.Height(50)))
            {
                ((DebugSettings)target).ResetSettings();
                EditorUtility.SetDirty((DebugSettings)target);
                AssetDatabase.SaveAssets();
            }
        }
    }
}
