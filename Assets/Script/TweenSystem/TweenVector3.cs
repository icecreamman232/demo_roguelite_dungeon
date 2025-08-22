using System;
using SGGames.Scripts.Core;
using UnityEngine;

namespace SGGames.Scripts.TweenSystem
{
    public class TweenVector3 : Tween
    {
        private Vector3 m_startValue;
        private Vector3 m_endValue;
        private Action<Vector3> m_onUpdate;
        
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        public TweenVector3(Vector3 start, Vector3 end,Action<Vector3> onUpdate,
            float duration, Global.EaseType easeType, AnimationCurve curve = null) 
            : base(duration, easeType, curve)
        {
            m_startValue = start;
            m_endValue = end;
            m_onUpdate = onUpdate;
        }

        public override bool Update(float deltaTime)
        {
            if (m_isFinished) return true;

            m_elapsedTime += deltaTime;
            float t = Mathf.Clamp01(m_elapsedTime / m_duration);
            float easeT = ApplyEasing(t);
            Vector3 value = Vector3.Lerp(m_startValue, m_endValue, easeT);
            m_onUpdate?.Invoke(value);
            
            if (t >= 1f)
            {
                m_isFinished = true;
                m_onFinished?.Invoke();
                return true;
            }
            
            return false;
        }
    }
}