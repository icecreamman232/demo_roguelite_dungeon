
using SGGames.Script.Events;
using UnityEngine;

namespace SGGames.Script.Core
{
    public class RoomObjectPooler : ObjectPooler
    {
        [SerializeField] private GameEvent m_gameEvent;

        protected override void Awake()
        {
            m_gameEvent.AddListener(OnReceiveGameEvent);
            base.Awake();
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
