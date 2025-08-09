using System.Collections;
using SGGames.Script.Core;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace SGGames.Script.UI
{
    public class LoadingScreenController : MonoBehaviour, IGameService
    {
        [SerializeField] private bool m_isDefaultFadeOut;
        [SerializeField] private CanvasGroup m_canvasGroup;
        [SerializeField] private LoadingScreenEvent m_loadingScreenEvent;
        [SerializeField] private AssetReference m_gameplayScene;
        [SerializeField] private AssetReference m_menuScene;

        private bool m_isLoading;
        public const float k_DefaultLoadingTime = 0.5f;
        public bool IsBlackOut => m_canvasGroup.alpha == 1;
        
        #region Unity Methods
        
        private void Awake()
        {
            ServiceLocator.RegisterService<LoadingScreenController>(this);
        }

        private void OnDestroy()
        {
            ServiceLocator.UnregisterService<LoadingScreenController>();
        }
        
        #endregion
        
        #region Loading Screens
        public void LoadBootstrapToMenu()
        {
            StartCoroutine(OnLoadingBootstrapToMenu());
        }

        private IEnumerator OnLoadingBootstrapToMenu()
        {
            var loadMenuSceneOpt = m_menuScene.LoadSceneAsync(LoadSceneMode.Additive);
            yield return new WaitUntil(()=> loadMenuSceneOpt.IsDone);

            SceneManager.SetActiveScene(loadMenuSceneOpt.Result.Scene);
            FadeInFromBlack();
            yield return new WaitForSeconds(k_DefaultLoadingTime);
            
            SceneManager.UnloadSceneAsync("BootstrapScene");
        }
        
        public void LoadMenuToGameplay()
        {
            StartCoroutine(OnLoadingMenuToGameplay());
        }

        private IEnumerator OnLoadingMenuToGameplay()
        {
            FadeOutToBlack();
            yield return new WaitForSeconds(k_DefaultLoadingTime);
            var unloadMenuSceneOpt = m_menuScene.UnLoadScene();
            yield return new WaitUntil(() => unloadMenuSceneOpt.IsDone);
            
            var loadGameplaySceneOpt = m_gameplayScene.LoadSceneAsync(LoadSceneMode.Additive);
            yield return new WaitUntil(() => loadGameplaySceneOpt.IsDone);
            SceneManager.SetActiveScene(loadGameplaySceneOpt.Result.Scene);
        }

        public void LoadGameplayToMenu()
        {
            StartCoroutine(OnLoadingGameplayToMenu());
        }

        private IEnumerator OnLoadingGameplayToMenu()
        {
            var loadMenuSceneOpt = m_menuScene.LoadSceneAsync(LoadSceneMode.Additive);
            yield return new WaitUntil(() => loadMenuSceneOpt.IsDone);
            SceneManager.SetActiveScene(loadMenuSceneOpt.Result.Scene);
            var unloadGameplaySceneOpt = m_gameplayScene.UnLoadScene();
            yield return new WaitUntil(() => unloadGameplaySceneOpt.IsDone);
            FadeInFromBlack();
        }
        
        #endregion
        
        #region Fading Effect

        public void FadeOutToBlack(float duration = 0.5f)
        {
            if (m_isLoading) return;
            StartCoroutine(OnFadeOut(duration));
        }

        public void FadeInFromBlack(float duration = 0.5f)
        {
            if (m_isLoading) return;
            StartCoroutine(OnFadeIn(duration));
        }
        
        private IEnumerator OnFadeOut(float duration)
        {
            m_isLoading = true;
            var timer = 0f;
            while (m_canvasGroup.alpha < 1)
            {
                timer += Time.unscaledDeltaTime;
                m_canvasGroup.alpha = MathHelpers.Remap(timer, 0, duration, 0, 1);
                yield return null;
            }
            m_canvasGroup.alpha = 1;
            m_isLoading = false;
        }
        
        private IEnumerator OnFadeIn(float duration)
        {
            m_isLoading = true;
            var timer = duration;
            while (m_canvasGroup.alpha > 0)
            {
                timer -= Time.unscaledDeltaTime;
                m_canvasGroup.alpha = MathHelpers.Remap(timer, 0, duration, 0, 1);
                yield return null;
            }
            m_canvasGroup.alpha = 0;
            m_isLoading = false;
        }
        
        #endregion
    }
}
