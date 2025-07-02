using System.Collections;
using SGGames.Script.Core;
using SGGames.Script.Events;
using SGGames.Script.UI;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace SGGames.Script.Managers
{
    public class LevelManager : MonoBehaviour, IGameService
    {
        [SerializeField] private AssetReferenceGameObject m_playerPrefab;
        [SerializeField] private GameEvent m_gameEvent;
        [SerializeField] private Transform m_spawnPosition;

        private readonly float DELAY_TIME = 0.5f;
        private WaitForSeconds m_delayCoroutine;
        
        private void Awake()
        {
            ServiceLocator.RegisterService<LevelManager>(this);
            m_delayCoroutine = new WaitForSeconds(DELAY_TIME);
        }

        private void Start()
        {
            StartCoroutine(LoadLevel());
        }
        
        private IEnumerator LoadLevel()
        {
            var loadingSceneController = ServiceLocator.GetService<LoadingScreenController>();
            var roomManager = ServiceLocator.GetService<RoomManager>();
            
            m_gameEvent.Raise(Global.GameEventType.SpawnPlayer);
            var loadingPlayerPrefabOperation = m_playerPrefab.LoadAssetAsync();
            yield return new WaitUntil(() => loadingPlayerPrefabOperation.IsDone);
            Instantiate(loadingPlayerPrefabOperation.Result, m_spawnPosition.position, Quaternion.identity);
            yield return new WaitForEndOfFrame();
            m_gameEvent.Raise(Global.GameEventType.PlayerCreated);

            var roomData = roomManager.GetNextLeftRoom();
            Instantiate(roomData.RoomPrefab);
            yield return new WaitForEndOfFrame();
            m_gameEvent.Raise(Global.GameEventType.RoomCreated);
            
            loadingSceneController.FadeInFromBlack();
            yield return m_delayCoroutine;
            m_gameEvent.Raise(Global.GameEventType.GameStarted);
        }
        
        #region TESTING
        
        [ContextMenu("Load Level")]
        private void TestLoadLevel()
        {
            StartCoroutine(LoadLevel());
        }
        
        #endregion
    }
}
