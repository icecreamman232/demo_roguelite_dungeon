using UnityEditor;
using UnityEditor.SceneManagement;

namespace SGGames.Script.EditorExtensions
{
    public class QuickOpenSceneEditor
    {
        [MenuItem("SGGames/Scene/Bootstrap")]
        public static void OpenBootstrapScene()
        {
            OpenScene("Assets/Scenes/BootstrapScene.unity");
        }
        
        [MenuItem("SGGames/Scene/MainMenu")]
        public static void OpenMainMenuScene()
        {
            OpenScene("Assets/Scenes/MainMenuScene.unity");
        }
        
        [MenuItem("SGGames/Scene/Gameplay")]
        public static void OpenGameplayScene()
        {
            OpenScene("Assets/Scenes/GameplayScene.unity");
        }
        
        [MenuItem("SGGames/Scene/Room Design")]
        public static void OpenRoomDesignScene()
        {
            OpenScene("Assets/Scenes/RoomDesignScene_EditorOnly.unity");
        }
        
        private static void OpenScene(string address)
        {
            EditorSceneManager.OpenScene(address, OpenSceneMode.Single);
        }
    }
}

