using System;
using SGGames.Script.Core;
using SGGames.Script.Events;
using UnityEngine;

namespace SGGames.Script.Managers
{
    public class GameManager : MonoBehaviour, IGameService
    {
        [SerializeField] private GameEvent m_gameEvent;
        
        public bool IsGamePaused => Time.timeScale == 0;

        public Action OnGamePauseCallback;
        public Action OnGameUnPauseCallback;
        
        private void Awake()
        {
            ServiceLocator.RegisterService<GameManager>(this);
            m_gameEvent.AddListener(OnReceiveGameEvent);
        }

        private void OnReceiveGameEvent(Global.GameEventType eventType)
        {
            if (eventType == Global.GameEventType.PauseGame)
            {
                PauseGame();
            }
            else if (eventType == Global.GameEventType.UnpauseGame)
            {
                UnpauseGame();
            }
        }

        private void PauseGame()
        {
            Time.timeScale = 0;
            OnGamePauseCallback?.Invoke();
        }

        private void UnpauseGame()
        {
            Time.timeScale = 1;
            OnGameUnPauseCallback?.Invoke();
        }
    }
}
