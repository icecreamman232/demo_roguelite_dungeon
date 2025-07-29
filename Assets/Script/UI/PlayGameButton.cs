
using System.Collections;
using SGGames.Script.Core;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace SGGames.Script.UI
{
    public class PlayGameButton : ButtonController
    {
        [SerializeField] private AssetReference m_gameplayScene;
        [SerializeField] private AssetReference m_mainMenuScene;

        private bool m_isLoading;

        private void LoadGameplayScene()
        {
            m_isLoading = true;
            StartCoroutine(OnLoadingScene());
        }

        private IEnumerator OnLoadingScene()
        {
            var loadingSceneController = ServiceLocator.GetService<LoadingScreenController>();
            loadingSceneController.FadeOutToBlack();
            yield return new WaitForSeconds(LoadingScreenController.k_DefaultLoadingTime);
            
            var loadGameplaySceneOpt = m_gameplayScene.LoadSceneAsync(LoadSceneMode.Additive);
            yield return new WaitUntil(() => loadGameplaySceneOpt.IsDone);
            SceneManager.SetActiveScene(loadGameplaySceneOpt.Result.Scene);
            
            var unloadMenuSceneOpt = SceneManager.UnloadSceneAsync("MainMenuScene");
            yield return new WaitUntil(()=> unloadMenuSceneOpt.isDone);
        }
        
        public override void OnPointerClick(PointerEventData eventData)
        {
            if (m_isLoading) return;
            LoadGameplayScene();
            base.OnPointerClick(eventData);
        }
    }
}

