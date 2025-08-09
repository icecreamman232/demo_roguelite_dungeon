using System;
using System.Collections;
using SGGames.Script.Core;
using SGGames.Script.UI;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace SGGames.Script.Managers
{
    public class BootstrapSystem : MonoBehaviour
    {
        [SerializeField] private AssetReference m_mainMenuScene;
        [SerializeField] private AssetReference m_loadingScene;

        private void Start()
        {
            InitializeSystem();
        }

        private void InitializeSystem()
        {
            StartCoroutine(OnRunningSystem());
        }

        private IEnumerator OnRunningSystem()
        {
            var loadLoadingSceneOpt = Addressables.LoadSceneAsync(m_loadingScene,LoadSceneMode.Additive, activateOnLoad:true);
            yield return new WaitUntil(()=> loadLoadingSceneOpt.IsDone);
            
            var loadingScreenController = ServiceLocator.GetService<LoadingScreenController>();
            loadingScreenController.LoadBootstrapToMenu();
            
            // var loadMenuSceneOpt = Addressables.LoadSceneAsync(m_mainMenuScene, LoadSceneMode.Additive);
            // yield return new WaitUntil(()=> loadMenuSceneOpt.IsDone);
            //
            // SceneManager.SetActiveScene(loadMenuSceneOpt.Result.Scene);
            //
            // yield return new WaitUntil(() => ServiceLocator.HasService<MainMenuController>());
            // var mainMenuController = ServiceLocator.GetService<MainMenuController>();
            // mainMenuController.LoadMenu();
            //
            // // Unload the bootstrap scene
            // var unloadBootstrapOpt = SceneManager.UnloadSceneAsync("BootstrapScene");
            // yield return new WaitUntil(()=> unloadBootstrapOpt.isDone);
        }
    }
}
