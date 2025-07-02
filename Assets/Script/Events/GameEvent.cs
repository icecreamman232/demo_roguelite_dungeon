using System;
using SGGames.Script.Core;
using UnityEngine;

namespace SGGames.Script.Events
{
    [CreateAssetMenu(menuName = "SGGames/Event/Game Event", fileName = "Game Event")]
    public class GameEvent : ScriptableObject
    {
        private Action<Global.GameEventType> m_listener;

        public void AddListener(Action<Global.GameEventType> addListener)
        {
            m_listener += addListener;
        }

        public void RemoveListener(Action<Global.GameEventType> removeListener)
        {
            m_listener -= removeListener;
        }

        public void Raise(Global.GameEventType gameEventType)
        {
            m_listener?.Invoke(gameEventType);
        }
    }
}