using System;
using SGGames.Script.Core;
using UnityEngine;

namespace SGGames.Script.TweenSystem
{
    public class TweenFloat : Tween
    {
        private float m_startValue;
        private float m_endValue;
        private Action<float> m_onUpdate;
        
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        public TweenFloat(float start, float end, Action<float> onUpdate,
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
            float value = Mathf.Lerp(m_startValue, m_endValue, easeT);
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
