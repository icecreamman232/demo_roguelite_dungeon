using System;
using SGGames.Script.Core;
using UnityEngine;

namespace SGGames.Script.Events
{
    [CreateAssetMenu(fileName = "Loading Screen Event", menuName = "SGGames/Event/Loading Screen")]
    public class LoadingScreenEvent : ScriptableObject
    {
        private Action<Global.LoadingScreenType> m_listener;

        public void AddListener(Action<Global.LoadingScreenType> addListener)
        {
            m_listener += addListener;
        }

        public void RemoveListener(Action<Global.LoadingScreenType> removeListener)
        {
            m_listener -= removeListener;
        }

        public void Raise(Global.LoadingScreenType loadingScreenType)
        {
            m_listener?.Invoke(loadingScreenType);
        }
    }
}
