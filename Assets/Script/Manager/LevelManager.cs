using System.Collections;
using SGGames.Script.Core;
using SGGames.Script.Data;
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
        [Header("Testing")]
        [SerializeField] private bool m_isUsingTestRoom;
        [SerializeField] private RoomData m_testRoom;

        private readonly float DELAY_TIME = 0.5f;
        private WaitForSeconds m_delayCoroutine;
        private bool m_isLoading;
        private GameObject m_player;
        
        
        private void Awake()
        {
            ServiceLocator.RegisterService<LevelManager>(this);
            m_delayCoroutine = new WaitForSeconds(DELAY_TIME);
            m_gameEvent.AddListener(OnReceiveGameEvent);
        }
        
        private void Start()
        {
            if (m_isLoading) return;
            StartCoroutine(LoadLevel(isFirstLoad:true));
        }
        
        private IEnumerator LoadLevel(bool isFirstLoad = false)
        {
            m_isLoading = true;
            
            var loadingSceneController = ServiceLocator.GetService<LoadingScreenController>();
            var roomManager = ServiceLocator.GetService<RoomManager>();
            
            m_gameEvent.Raise(Global.GameEventType.PauseGame);
            
            if (isFirstLoad)
            {
                loadingSceneController.FadeOutToBlack();
                
                m_gameEvent.Raise(Global.GameEventType.SpawnPlayer);
                var loadingPlayerPrefabOperation = m_playerPrefab.LoadAssetAsync();
                yield return new WaitUntil(() => loadingPlayerPrefabOperation.IsDone);
                m_player = Instantiate(loadingPlayerPrefabOperation.Result, m_spawnPosition.position, Quaternion.identity);
                yield return new WaitForEndOfFrame();
                m_gameEvent.Raise(Global.GameEventType.PlayerCreated);
            }
            else
            {
                m_player.transform.position = m_spawnPosition.position;
            }
            
            #if UNITY_EDITOR
            if (m_isUsingTestRoom)
            {
                Instantiate(m_testRoom.RoomPrefab);
            }
            else
            {
                var roomData = roomManager.GetNextLeftRoom();
                Instantiate(roomData.RoomPrefab);
            }
            #else
            var roomData = roomManager.GetNextLeftRoom();
            Instantiate(roomData.RoomPrefab);
            #endif
            
            yield return new WaitForEndOfFrame();
            m_gameEvent.Raise(Global.GameEventType.RoomCreated);
            
            m_gameEvent.Raise(Global.GameEventType.UnpauseGame);
            loadingSceneController.FadeInFromBlack();
            yield return m_delayCoroutine;
            m_gameEvent.Raise(Global.GameEventType.GameStarted);

            m_isLoading = false;
        }
        
        private void OnReceiveGameEvent(Global.GameEventType eventType)
        {
            if (eventType == Global.GameEventType.LoadNextRoom)
            {
                if (m_isLoading) return;
                StartCoroutine(LoadLevel());
            }
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
