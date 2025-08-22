using SGGames.Scripts.HealthSystem;
using UnityEditor;
using UnityEngine;

namespace SGGames.Scripts.EditorExtensions
{
    [CustomEditor(typeof(PlayerHealth))]
    public class PlayerHealthInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Kill", GUILayout.Height(30)))
            {
                ((PlayerHealth)target).KillPlayer();
            }
        }
    }
}

