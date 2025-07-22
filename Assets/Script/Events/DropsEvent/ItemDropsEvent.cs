using System;

using UnityEngine;

namespace SGGames.Script.Events
{
    [CreateAssetMenu(fileName ="Item Drops Event", menuName = "SGGames/Event/Item Drops ")]
    public class ItemDropsEvent : ScriptableObject
    {
        protected Action<Vector3> m_listener;

        public virtual void AddListener(Action<Vector3> addListener)
        {
            m_listener += addListener;
        }

        public virtual void RemoveListener(Action<Vector3> removeListener)
        {
            m_listener -= removeListener;
        }

        public virtual void Raise(Vector3 dropPosition)
        {
            m_listener?.Invoke(dropPosition);
        }
    }
}
