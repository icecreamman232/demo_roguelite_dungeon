using System.Collections;
using SGGames.Scripts.Core;
using UnityEngine;

namespace SGGames.Scripts.UI
{
    public class MainMenuController : MonoBehaviour, IGameService
    {
        private void Awake()
        {
            ServiceLocator.RegisterService<MainMenuController>(this);
        }

        private void OnDestroy()
        {
            ServiceLocator.UnregisterService<MainMenuController>();
        }

        public void LoadMenu()
        {
            StartCoroutine(OnLoadToMenu());
        }

        private IEnumerator OnLoadToMenu()
        {
            var loadingController = ServiceLocator.GetService<LoadingScreenController>();
            loadingController.FadeInFromBlack();
            yield return new WaitForSeconds(LoadingScreenController.k_DefaultLoadingTime);
        }
    }
}
