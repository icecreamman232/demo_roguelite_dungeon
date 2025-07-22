using System;
using UnityEngine;

namespace SGGames.Script.Events
{
    [CreateAssetMenu(menuName = "SGGames/Event/Spawn Chest", fileName = "Spawn Chest Event")]
    public class SpawnChestEvent : ScriptableObject
    {
        private Action<Vector3> m_listener;

        public void AddListener(Action<Vector3> addListener)
        {
            m_listener += addListener;
        }

        public void RemoveListener(Action<Vector3> removeListener)
        {
            m_listener -= removeListener;
        }

        public void Raise(Vector3 spawnPosition)
        {
            m_listener?.Invoke(spawnPosition);
        }
    }
}
