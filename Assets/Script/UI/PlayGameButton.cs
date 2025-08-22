using SGGames.Scripts.Core;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;

namespace SGGames.Scripts.UI
{
    public class PlayGameButton : ButtonController
    {
        [SerializeField] private AssetReference m_gameplayScene;
        [SerializeField] private AssetReference m_mainMenuScene;

        private bool m_isLoading;

        private void LoadGameplayScene()
        {
            m_isLoading = true;
            var loadingSceneController = ServiceLocator.GetService<LoadingScreenController>();
            loadingSceneController.LoadMenuToGameplay();
        }
        
        public override void OnPointerClick(PointerEventData eventData)
        {
            if (m_isLoading) return;
            LoadGameplayScene();
            base.OnPointerClick(eventData);
        }
    }
}

