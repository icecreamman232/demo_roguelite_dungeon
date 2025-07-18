using System.Collections;
using SGGames.Script.Core;
using SGGames.Script.Data;
using SGGames.Script.Dungeon;
using SGGames.Script.EditorExtensions;
using SGGames.Script.Events;
using SGGames.Script.UI;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace SGGames.Script.Managers
{
    public class LevelManager : MonoBehaviour, IGameService
    {
        [SerializeField] private AssetReferenceGameObject m_playerPrefab;
        [SerializeField] private BiomesTransitionUIEvent m_biomesTransitionUIEvent;
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
            ConsoleCheatManager.RegisterCommands(this);
        }
        
        private void Start()
        {
            if (m_isLoading) return;
            StartCoroutine(LoadLevel(fromLeftRoom:true, isFirstLoad:true));
        }
        
        private IEnumerator LoadLevel(bool fromLeftRoom, bool shouldGenerateRoom = false, bool isFirstLoad = false)
        {
            m_isLoading = true;
            
            var loadingSceneController = ServiceLocator.GetService<LoadingScreenController>();
            var roomManager = ServiceLocator.GetService<RoomManager>();
            var inputManager = ServiceLocator.GetService<InputManager>();
            
            m_gameEvent.Raise(Global.GameEventType.PauseGame);
            inputManager.DisableInput();
            
            if (isFirstLoad)
            {
                yield return StartCoroutine(FadeOutToBlack(loadingSceneController));
                yield return StartCoroutine(GenerateRoomsForBiomes(roomManager));
                yield return StartCoroutine(CreatePlayerFirstTime());   
            }
            else
            {
                yield return StartCoroutine(FadeOutToBlack(loadingSceneController));

                if (shouldGenerateRoom)
                {
                    yield return StartCoroutine(GenerateRoomsForBiomes(roomManager));
                }
                
                yield return StartCoroutine(MovePlayerToSpawnPosition());
            }
            
            yield return StartCoroutine(InstantiateRoom(fromLeftRoom, roomManager));
            m_gameEvent.Raise(Global.GameEventType.RoomCreated);
            m_gameEvent.Raise(Global.GameEventType.UnpauseGame);
            yield return StartCoroutine(FadeInFromBlack(loadingSceneController));
            m_gameEvent.Raise(Global.GameEventType.GameStarted);
            inputManager.EnableInput();
            
            m_isLoading = false;
        }

        private IEnumerator PrepareLoadNextBiomes()
        {
            m_isLoading = true;
            
            var loadingSceneController = ServiceLocator.GetService<LoadingScreenController>();
            var roomManager = ServiceLocator.GetService<RoomManager>();
            var inputManager = ServiceLocator.GetService<InputManager>();
            
            inputManager.DisableInput();
            yield return StartCoroutine(FadeOutToBlack(loadingSceneController));
            yield return StartCoroutine(MovePlayerToSpawnPosition());
            roomManager.IncreaseBiomeIndex();
            m_biomesTransitionUIEvent.Raise(roomManager.CurrentBiomesIndex);
            
            m_isLoading = false;
        }
        
        #region Coroutines

        private IEnumerator FadeInFromBlack(LoadingScreenController loadingSceneController)
        {
            loadingSceneController.FadeInFromBlack();
            yield return new WaitForSecondsRealtime(DELAY_TIME);
        }

        private IEnumerator FadeOutToBlack(LoadingScreenController loadingSceneController)
        {
            loadingSceneController.FadeOutToBlack();
            yield return new WaitForSecondsRealtime(DELAY_TIME);
        }

        private IEnumerator GenerateRoomsForBiomes(RoomManager roomManager)
        {
            roomManager.GenerateRoomForCurrentBiomes();
            roomManager.GenerateRoomRewardForCurrentBiomes();
            yield return new WaitForEndOfFrame();
        }

        private IEnumerator InstantiateRoom(bool fromLeftRoom, RoomManager roomManager)
        {
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
        }

        private IEnumerator CreatePlayerFirstTime()
        {
            m_gameEvent.Raise(Global.GameEventType.SpawnPlayer);
            var loadingPlayerPrefabOperation = m_playerPrefab.LoadAssetAsync();
            yield return new WaitUntil(() => loadingPlayerPrefabOperation.IsDone);
            m_player = Instantiate(loadingPlayerPrefabOperation.Result, m_spawnPosition.position, Quaternion.identity);
            yield return new WaitForEndOfFrame();
            m_gameEvent.Raise(Global.GameEventType.PlayerCreated);
        }

        private IEnumerator MovePlayerToSpawnPosition()
        {
            m_gameEvent.Raise(Global.GameEventType.SpawnPlayer);
            m_player.transform.position = m_spawnPosition.position;
            yield return new WaitForSecondsRealtime(0.3f); //Small delay to feel better after moving player
            m_gameEvent.Raise(Global.GameEventType.PlayerCreated);
        }
        
        #endregion
        
        private void OnReceiveGameEvent(Global.GameEventType eventType)
        {
            switch (eventType)
            {
                case Global.GameEventType.LoadNextRoomLeftRoom:
                    if (m_isLoading) return;
                    StartCoroutine(LoadLevel(fromLeftRoom:true));
                    break;
                case Global.GameEventType.LoadNextRoomRightRoom:
                    if (m_isLoading) return;
                    StartCoroutine(LoadLevel(fromLeftRoom:false));
                    break;
                case Global.GameEventType.PlayBiomesTransition:
                    if (m_isLoading) return;
                    StartCoroutine(PrepareLoadNextBiomes());
                    break;
                case Global.GameEventType.LoadNextBiomes:
                    if (m_isLoading) return;
                    StartCoroutine(LoadLevel(fromLeftRoom:true, shouldGenerateRoom:true));
                    break;
            }
        }

        
        #region TESTING

        [CheatCode("KillAll","Kill all enemies in room")]
        private void KillAllEnemiesInRoom()
        {
            var roomController = m_currentRoom.GetComponent<Room>();
            roomController.KillAllEnemiesInRoom();
        }
        
        [ContextMenu("Load Level")]
        private void TestLoadLevel()
        {
            StartCoroutine(LoadLevel(fromLeftRoom:true));
        }
        
        #endregion
    }
}
