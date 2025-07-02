using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace SGGames.Script.Managers
{
    public class BootstrapSystem : MonoBehaviour
    {
        [SerializeField] private AssetReference m_mainMenuScene;

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
            var loadMenuSceneOperation = Addressables.LoadSceneAsync(m_mainMenuScene);
            yield return new WaitUntil(()=> loadMenuSceneOperation.IsDone);
        }
    }
}
