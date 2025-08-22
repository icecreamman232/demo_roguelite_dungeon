using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SGGames.Scripts.Modules
{
    [InitializeOnLoad]
    public class SaveRoomDesignSceneEditor
    {
        private static string m_roomDesignSceneName = "RoomDesignScene_EditorOnly";
        private static RoomDesignSaveSystem m_saveSystem;
        private static float BUTTON_WIDTH = 200;
        private static float BUTTON_HEIGHT = 30;
        private static float INDENT_SIZE = 10;
        
        static SaveRoomDesignSceneEditor()
        {
            //Catch the event when switching scene
            EditorApplication.hierarchyChanged += OnHierarchyChanged;
        }

        private static bool IsRoomDesignSceneActive()
        {
            var activeScene = SceneManager.GetActiveScene();
            return activeScene.name == m_roomDesignSceneName;
        }

        private static void OnHierarchyChanged()
        {
            //Only show the toolbar if its design scene
            if (IsRoomDesignSceneActive())
            {
                SceneView.duringSceneGui += OnSceneGUI;
            }
            else
            {
                SceneView.duringSceneGui -= OnSceneGUI;
            }
        }

        private static void OnSceneGUI(SceneView sceneView)
        {
            GUI.BeginGroup(new Rect(sceneView.position.width - 220 ,10,200,120),"Room Design Toolbar",GUI.skin.window);
            
            if (GUI.Button(new Rect(INDENT_SIZE ,BUTTON_HEIGHT,BUTTON_WIDTH - INDENT_SIZE * 2, BUTTON_HEIGHT),"Save"))
            {
                CallToMethod("SaveLevel");
            }
            
            if (GUI.Button(new Rect(INDENT_SIZE ,BUTTON_HEIGHT * 2 + INDENT_SIZE,BUTTON_WIDTH - INDENT_SIZE * 2, BUTTON_HEIGHT),"Reset"))
            {
                CallToMethod("ResetLevel");
            }
            
            GUI.EndGroup();
        }

        private static void CallToMethod(string methodName)
        {
            if (m_saveSystem == null)
            {
                m_saveSystem = Object.FindAnyObjectByType<RoomDesignSaveSystem>();
            }
            var saveMethod = typeof(RoomDesignSaveSystem).GetMethod(methodName,
                BindingFlags.Instance | BindingFlags.Public);
            saveMethod?.Invoke(m_saveSystem, null);
        }
    }
}

