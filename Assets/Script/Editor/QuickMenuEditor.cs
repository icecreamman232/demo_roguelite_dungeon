using SGGames.Scripts.AI;
using SGGames.Scripts.Entity;
using UnityEditor;
using UnityEngine;

namespace SGGames.Script.EditorExtensions
{
    public partial class QuickMenuEditor
    {
        [MenuItem("GameObject/Create Enemy Brain",priority = 0)]
        public static void AddingEmptyEnemyBrain()
        {
            GameObject brain = new GameObject();
            brain.name = "New Empty Brain";
            brain.AddComponent<AIBrain>();
            
            GameObject selection = Selection.activeGameObject;
            brain.transform.SetParent(selection.transform,true);
            
            GameObject action = new GameObject();
            action.name = "Actions";
            action.transform.SetParent(brain.transform);
            
            GameObject decision = new GameObject();
            decision.name = "Decisions";
            decision.transform.SetParent(brain.transform);
        }

        [MenuItem("GameObject/Create Item",priority = 1)]
        public static void AddInventoryItemTemplate()
        {
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefab/Pickable/InventoryItemTemplate.prefab");
            PrefabUtility.InstantiatePrefab(prefab);
        }
    }
}