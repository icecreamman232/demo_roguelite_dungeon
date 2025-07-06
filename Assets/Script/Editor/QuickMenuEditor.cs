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
            brain.AddComponent<EnemyBrain>();
            
            GameObject selection = Selection.activeGameObject;
            brain.transform.SetParent(selection.transform);
            
            GameObject action = new GameObject();
            action.name = "Actions";
            action.transform.SetParent(brain.transform);
            
            GameObject decision = new GameObject();
            decision.name = "Decisions";
            decision.transform.SetParent(brain.transform);
        }
    }
}