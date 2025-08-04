using System;
using SGGames.Script.Core;
using UnityEngine;

namespace SGGames.Script.Items
{
    [CreateAssetMenu(fileName = "World Event", menuName = "SGGames/Event/World Event")]
    public class WorldEvent : ScriptableObject
    {
        private Action<Global.WorldEventType, GameObject> m_listener;

        public void AddListener(Action<Global.WorldEventType, GameObject> addListener)
        {
            m_listener += addListener;
        }

        public void RemoveListener(Action<Global.WorldEventType, GameObject> removeListener)
        {
            m_listener -= removeListener;
        }

        public void Raise(Global.WorldEventType worldEventType, GameObject source)
        {
            m_listener?.Invoke(worldEventType, source);
        }
    }
}
