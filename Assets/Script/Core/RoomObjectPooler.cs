
using System;
using SGGames.Scripts.Events;
using UnityEngine;

namespace SGGames.Scripts.Core
{
    public class RoomObjectPooler : ObjectPooler
    {
        [SerializeField] private GameEvent m_gameEvent;

        protected override void Awake()
        {
            m_gameEvent.AddListener(OnReceiveGameEvent);
            base.Awake();
        }

        private void OnDestroy()
        {
            m_gameEvent.RemoveListener(OnReceiveGameEvent);
        }

        private void OnReceiveGameEvent(Global.GameEventType gameEvent)
        {
            if (gameEvent == Global.GameEventType.LoadNextRoomLeftRoom || gameEvent == Global.GameEventType.LoadNextRoomRightRoom)
            {
                CleanUp();
            }
        }
    }
}
