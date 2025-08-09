using System;
using SGGames.Script.Core;
using UnityEngine;

namespace SGGames.Script.Skills
{
    [CreateAssetMenu(fileName = "Trigger Impact Event", menuName = "SGGames/Event/Trigger Impact")]
    public class TriggerImpactEvent : ScriptableObject
    {
        private Action<Vector3, ImpactParamInfo> m_listener;

        public void AddListener(Action<Vector3, ImpactParamInfo> addListener)
        {
            m_listener += addListener;
        }

        public void RemoveListener(Action<Vector3, ImpactParamInfo> removeListener)
        {
            m_listener -= removeListener;
        }

        public void Raise(Vector3 position, ImpactParamInfo paramInfo)
        {
            m_listener?.Invoke(position, paramInfo);
        }
    }

    public abstract class ImpactParamInfo : ScriptableObject
    {
        public Global.ImpactID ImpactID;
        public Vector3 Position;
    }
}
