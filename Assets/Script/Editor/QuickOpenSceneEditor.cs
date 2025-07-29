using SGGames.Script.UI;
using UnityEditor;
using UnityEditor.SceneManagement;
using Object = UnityEngine.Object;

namespace SGGames.Script.EditorExtensions
{
    public class QuickOpenSceneEditor
    {
        [MenuItem("SGGames/Scene/Bootstrap")]
        public static void OpenBootstrapScene()
        {
            OpenScene("Assets/Scenes/BootstrapScene.unity", isSingle:true,setToActive:true);
        }
        
        [MenuItem("SGGames/Scene/MainMenu")]
        public static void OpenMainMenuScene()
        {
            OpenScene("Assets/Scenes/MainMenuScene.unity", isSingle:true, setToActive:true);
            OpenScene("Assets/Scenes/PermanentScene.unity", isSingle:false);
            var loadingSceneController = Object.FindFirstObjectByType<LoadingScreenController>();
            if (loadingSceneController.IsBlackOut)
            {
                loadingSceneController.FadeInFromBlack();
            }
        }
        
        [MenuItem("SGGames/Scene/Gameplay")]
        public static void OpenGameplayScene()
        {
            OpenScene("Assets/Scenes/GameplayScene.unity", isSingle:true, setToActive:true);
            OpenScene("Assets/Scenes/PermanentScene.unity", isSingle:false);
        }
        
        [MenuItem("SGGames/Scene/Room Design")]
        public static void OpenRoomDesignScene()
        {
            OpenScene("Assets/Scenes/RoomDesignScene_EditorOnly.unity", isSingle:true, setToActive:true);
        }
        
        private static void OpenScene(string address,bool isSingle, bool setToActive = false)
        {
            var scene = EditorSceneManager.OpenScene(address,isSingle ? OpenSceneMode.Single : OpenSceneMode.Additive);
            if (setToActive)
            {
                EditorSceneManager.SetActiveScene(scene);
            }
        }
    }
}

