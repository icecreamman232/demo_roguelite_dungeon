using System.Collections;
using SGGames.Script.Data;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SGGames.Script.Core
{
    /// <summary>
    /// Use to initialize all systems to prevent race condition
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
            //Waiting for the scene which contains the bootstrap to be activated and setup completely
            //before initializing to avoid creating objects in an incorrect scene
            yield return new WaitUntil(()=>SceneManager.GetActiveScene() == gameObject.scene);

            
            
            for (int i = 0; i < m_profile.AssetList.Length; i++)
            {
                if (m_profile.AssetList[i] == null)
                {
                    Debug.LogError($"Null asset in {this.name}");
                    continue;
                }
                
                var handle = m_profile.AssetList[i].LoadAssetAsync();
                yield return new WaitUntil(() => handle.IsDone);
                var createdGameObject = Instantiate(handle.Result);
                SceneManager.MoveGameObjectToScene(createdGameObject, gameObject.scene);
                FormatObjectName(createdGameObject, i);
            }
            

            Destroy(this.gameObject);
        }

        private void FormatObjectName(GameObject inputGO,int index)
        {
            var initName = inputGO.name;
            var updateName = initName.Replace("(Clone)", "");
            updateName = $"{index}-{updateName}";
            inputGO.name = updateName;
        }
    }
}

