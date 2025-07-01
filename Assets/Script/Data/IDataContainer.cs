using System.Collections.Generic;
using UnityEngine;

namespace SGGames.Script.Data
{
    public class IDataContainer<T> : ScriptableObject
    {
        [SerializeField] protected List<T> m_container = new List<T>();

        public List<T> GetContainer => m_container;
        
        public T GetItemAt(int index)
        {
            return m_container[index];
        }

        public int GetIndexOfItem(T item)
        {
            return m_container.IndexOf(item);
        }

        public void AddItem(T item)
        {
            m_container.Add(item);
        }

        public void ClearContainer()
        {
            m_container.Clear();
        }
    }
}
