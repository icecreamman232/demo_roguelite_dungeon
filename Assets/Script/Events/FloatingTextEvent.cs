using System;
using UnityEngine;

namespace SGGames.Script.Data
{
    [CreateAssetMenu(fileName = "Floating Text Event", menuName = "SGGames/Event/Floating Text")]
    public class FloatingTextEvent: ScriptableObject
    {
        private Action<string, Vector3> m_listener;

        public void AddListener(Action<string, Vector3> addListener)
        {
            m_listener += addListener;
        }

        public void RemoveListener(Action<string, Vector3>removeListener)
        {
            m_listener -= removeListener;
        }

        public void Raise(string content, Vector3 position)
        {
            m_listener?.Invoke(content, position);
        }
    }
}