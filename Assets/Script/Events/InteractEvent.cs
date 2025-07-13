using System;
using SGGames.Script.Core;
using UnityEngine;

namespace SGGames.Script.Events
{
    [CreateAssetMenu(fileName = "Interact Event", menuName = "SGGames/Event/Interact Event")]
    public class InteractEvent : ScriptableObject
    {
        private Action<Global.InteractEventType, int, string> m_listener;

        public void AddListener(Action<Global.InteractEventType, int, string> addListener)
        {
            m_listener += addListener;
        }

        public void RemoveListener(Action<Global.InteractEventType, int, string> removeListener)
        {
            m_listener -= removeListener;
        }

        public void Raise(Global.InteractEventType interactEventType, int layer, string tag)
        {
            m_listener?.Invoke(interactEventType, layer, tag);
        }
    }
}

