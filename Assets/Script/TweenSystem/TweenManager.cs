using System;
using System.Collections.Generic;
using SGGames.Script.Core;
using UnityEngine;

namespace SGGames.Script.TweenSystem
{
    public class TweenManager : PersistentSingleton<TweenManager>
    {
        private List<Tween> m_activeTweens;

        private void Start()
        {
            m_activeTweens = new List<Tween>();
        }

        private void Update()
        {
            for (int i = m_activeTweens.Count - 1; i >= 0; i--)
            {
                if (m_activeTweens[i].Update(Time.deltaTime))
                {
                    m_activeTweens.RemoveAt(i);
                }
            }
        }

        public void CancelTween(Tween tween)
        {
            m_activeTweens.Remove(tween);
        }
        
        public Tween TweenFloat(float start, float end,Action<float> onUpdate, float duration, Global.EaseType easeType, AnimationCurve curve = null)
        {
            TweenFloat tween = new TweenFloat(start, end,onUpdate, duration, easeType, curve);
            m_activeTweens.Add(tween);
            return tween;
        }
        
        public Tween TweenVector3(Vector3 start, Vector3 end,Action<Vector3> onUpdate, float duration, Global.EaseType easeType, AnimationCurve curve = null)
        {
            TweenVector3 tween = new TweenVector3(start, end, onUpdate, duration, easeType, curve);
            m_activeTweens.Add(tween);
            return tween;
        }
    }
}

