
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace SGGames.Script.UI
{
    public class PlayGameButton : ButtonController
    {
        [SerializeField] private AssetReference m_gameplayScene;

        private bool m_isLoading;
        
        private void LoadGameplayScene()
        {
            m_isLoading = true;
            StartCoroutine(OnLoadingScene());
        }

        private IEnumerator OnLoadingScene()
        {
            var asyncLoadSceneAssetOperationHandle = m_gameplayScene.LoadSceneAsync();
            yield return new WaitUntil(() => asyncLoadSceneAssetOperationHandle.IsDone);
            var loadSceneOperation = Addressables.LoadSceneAsync(asyncLoadSceneAssetOperationHandle.Result.Scene);
            yield return new WaitUntil(() => loadSceneOperation.IsDone);
        }
        
        public override void OnPointerClick(PointerEventData eventData)
        {
            if (m_isLoading) return;
            LoadGameplayScene();
            base.OnPointerClick(eventData);
        }
    }
}

