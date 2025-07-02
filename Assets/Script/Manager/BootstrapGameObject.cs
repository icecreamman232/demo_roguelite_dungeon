using System.Collections;
using SGGames.Script.Data;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace SGGames.Script.Core
{
    /// <summary>
    /// Use to initialize all system in order to prevent race condition
    /// </summary>
    public class BootstrapGameObject : MonoBehaviour
    {
        [SerializeField] private BootstrapProfile m_profile;
        
        private void Awake()
        {
            Initialize();
        }

        [ContextMenu("Initialize")]
        private void Initialize()
        {
            StartCoroutine(OnInitializeAsync());
        }

        private IEnumerator OnInitializeAsync()
        {
            AsyncOperationHandle<GameObject> nextOperationHandle;
            foreach (var asset in m_profile.AssetList)
            {
                if (asset == null)
                {
                    Debug.LogError($"Null asset in {this.name}");
                    continue;
                }

                nextOperationHandle = asset.LoadAssetAsync();
                var handle = nextOperationHandle;
                yield return new WaitUntil(() => handle.IsDone);
                Instantiate(handle.Result);
            }

            Destroy(this.gameObject);
        }
    }
}

