using System;
using UnityEngine;

namespace SGGames.Script.Data
{
    [CreateAssetMenu(fileName = "Biomes Transition UI Event", menuName = "SGGames/Event/Biomes Transition UI")]
    public class BiomesTransitionUIEvent: ScriptableObject
    {
        protected Action<int> m_listener;

        public virtual void AddListener(Action<int> addListener)
        {
            m_listener += addListener;
        }

        public virtual void RemoveListener(Action<int> removeListener)
        {
            m_listener -= removeListener;
        }

        public virtual void Raise(int biomesIndex)
        {
            m_listener?.Invoke(biomesIndex);
        }
    }
}