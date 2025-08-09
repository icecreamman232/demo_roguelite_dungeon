using System;
using UnityEngine;

namespace SGGames.Script.Data
{
    [CreateAssetMenu(fileName = "Biomes Transition UI Event", menuName = "SGGames/Event/Biomes Transition UI")]
    public class BiomesTransitionUIEvent: ScriptableObject
    {
        private Action<int> m_listener;

        public void AddListener(Action<int> addListener)
        {
            m_listener += addListener;
        }

        public void RemoveListener(Action<int> removeListener)
        {
            m_listener -= removeListener;
        }

        public void Raise(int biomesIndex)
        {
            m_listener?.Invoke(biomesIndex);
        }
    }
}