using System;
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
        private bool m_isLoading;
        private GameObject m_player;
        private GameObject m_currentRoom;
        private Rect m_normalRoomRect;
        
        public GameObject Player => m_player;
        public GameObject CurrentRoom => m_currentRoom;
        
        public Rect NormalRoomRect => m_normalRoomRect;
        
        private void Awake()
        {
            ServiceLocator.RegisterService<LevelManager>(this);
            m_gameEvent.AddListener(OnReceiveGameEvent);
            
            m_normalRoomRect = new Rect(m_spawnPosition.position.x, m_spawnPosition.position.y + 0.5f, 14, 7);
        }
        
        private void Start()
        {
            if (m_isLoading) return;
            StartCoroutine(LoadLevel(fromLeftRoom:true, isFirstLoad:true));
        }
        
        private IEnumerator LoadLevel(bool fromLeftRoom, bool isFirstLoad = false)
        {
            m_isLoading = true;
            
            var loadingSceneController = ServiceLocator.GetService<LoadingScreenController>();
            var roomManager = ServiceLocator.GetService<RoomManager>();
            
            m_gameEvent.Raise(Global.GameEventType.PauseGame);
            
            if (isFirstLoad)
            {
                loadingSceneController.FadeOutToBlack();
                yield return new WaitForSecondsRealtime(DELAY_TIME);
                m_gameEvent.Raise(Global.GameEventType.SpawnPlayer);
                var loadingPlayerPrefabOperation = m_playerPrefab.LoadAssetAsync();
                yield return new WaitUntil(() => loadingPlayerPrefabOperation.IsDone);
                m_player = Instantiate(loadingPlayerPrefabOperation.Result, m_spawnPosition.position, Quaternion.identity);
                yield return new WaitForEndOfFrame();
                m_gameEvent.Raise(Global.GameEventType.PlayerCreated);
            }
            else
            {
                loadingSceneController.FadeOutToBlack();
                yield return new WaitForSecondsRealtime(DELAY_TIME);
                m_player.transform.position = m_spawnPosition.position;
                yield return new WaitForSecondsRealtime(0.3f); //Small delay to feel better after moving player
            }
            
            #if UNITY_EDITOR
            if (m_isUsingTestRoom)
            {
                m_currentRoom = Instantiate(m_testRoom.RoomPrefab);
            }
            else
            {
                var roomData = fromLeftRoom ? roomManager.GetNextLeftRoom() : roomManager.GetNextRightRoom();
                m_currentRoom = Instantiate(roomData.RoomPrefab);
            }
            #else
            var roomData = fromLeftRoom ? roomManager.GetNextLeftRoom() : roomManager.GetNextRightRoom();;
            m_currentRoom = Instantiate(roomData.RoomPrefab);
            #endif
            
            yield return new WaitForEndOfFrame();
            m_gameEvent.Raise(Global.GameEventType.RoomCreated);
            m_gameEvent.Raise(Global.GameEventType.UnpauseGame);
            loadingSceneController.FadeInFromBlack();
            yield return new WaitForSecondsRealtime(DELAY_TIME);
            m_gameEvent.Raise(Global.GameEventType.GameStarted);

            m_isLoading = false;
        }

        private IEnumerator PrepareLoadNextBiomes()
        {
            m_isLoading = true;
            var loadingSceneController = ServiceLocator.GetService<LoadingScreenController>();
            
            loadingSceneController.FadeOutToBlack();
            yield return new WaitForSecondsRealtime(DELAY_TIME);
           
            m_player.transform.position = m_spawnPosition.position;
            yield return new WaitForSecondsRealtime(0.3f); //Small delay to feel better after moving player
            m_isLoading = false;
        }
        
        private void OnReceiveGameEvent(Global.GameEventType eventType)
        {
            if (eventType == Global.GameEventType.LoadNextRoomLeftRoom)
            {
                if (m_isLoading) return;
                StartCoroutine(LoadLevel(fromLeftRoom:true));
            }
            else if (eventType == Global.GameEventType.LoadNextRoomRightRoom)
            {
                if (m_isLoading) return;
                StartCoroutine(LoadLevel(fromLeftRoom:false));
            }
            else if (eventType == Global.GameEventType.LoadNextBiomes)
            {
                if (m_isLoading) return;
                StartCoroutine(PrepareLoadNextBiomes());
            }
        }

        
        #region TESTING
        
        [ContextMenu("Load Level")]
        private void TestLoadLevel()
        {
            StartCoroutine(LoadLevel(fromLeftRoom:true));
        }
        
        #endregion
    }
}
