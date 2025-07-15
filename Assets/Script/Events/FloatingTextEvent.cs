using System;
using UnityEngine;

namespace SGGames.Script.Data
{
    [CreateAssetMenu(fileName = "Floating Text Event", menuName = "SGGames/Event/Floating Text")]
    public class FloatingTextEvent: ScriptableObject
    {
        protected Action<string, Vector3> m_listener;

        public virtual void AddListener(Action<string, Vector3> addListener)
        {
            m_listener += addListener;
        }

        public virtual void RemoveListener(Action<string, Vector3>removeListener)
        {
            m_listener -= removeListener;
        }

        public virtual void Raise(string content, Vector3 position)
        {
            m_listener?.Invoke(content, position);
        }
    }
}