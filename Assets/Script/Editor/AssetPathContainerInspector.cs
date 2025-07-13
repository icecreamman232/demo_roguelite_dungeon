using SGGames.Script.Data;
using UnityEditor;
using UnityEngine;

namespace SGGames.Script.EditorExtensions
{
    [CustomEditor(typeof(AssetPathContainer))]
    public class AssetPathContainerInspector : Editor
    {
        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Check Paths"))
            {
                var container = (AssetPathContainer)target;
                CheckPath(container.EnemyHPBarPath , container, "Enemy HP Bar Path is not found");
                CheckPath(container.FillColorSpritePath , container, "Fill Color Sprite Path is not found");
                CheckPath(container.LootTablePath , container, "Loot Table Path is not found");
            }
        }

        private void CheckPath(string path, AssetPathContainer container, string errorMessage)
        {
            bool isExist = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (!isExist)
            {
                Debug.LogError(errorMessage);
            }
        }
    }
}
