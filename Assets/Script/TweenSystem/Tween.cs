using System;
using SGGames.Scripts.Core;
using UnityEngine;

namespace SGGames.Scripts.TweenSystem
{
    public abstract class Tween
    {
       protected float m_duration;
       protected float m_elapsedTime;
       protected Global.EaseType m_easeType;
       protected AnimationCurve m_curve;
       protected Action m_onFinished;
       protected bool m_isFinished;
       
       public Tween(float duration, Global.EaseType easeType, AnimationCurve curve = null)
       {
           m_duration = duration;
           m_easeType = easeType;
           m_curve = curve;
           m_elapsedTime = 0f;
           m_isFinished = false;
           
       }

       public Tween OnComplete(Action callback)
       {
           m_onFinished = callback;
           return this;
       }
       
       
       
       public abstract bool Update(float deltaTime);

       private const float c1 = 1.70158f;
       private const float c2 = c1 * 1.525f;
       private const float c3 = c1 + 1;
       private const float c4 = (2 * Mathf.PI) / 3;
       private const float c5 = (2 * Mathf.PI) / 4.5f;
       
       protected float ApplyEasing(float t)
       {
           switch (m_easeType)
           {
               case Global.EaseType.Linear:
                   return t;
               case Global.EaseType.EaseInSine:
                   return 1f - Mathf.Cos(t * Mathf.PI / 2f);
               case Global.EaseType.EaseOutSine:
                   return Mathf.Sin(t * Mathf.PI / 2f);
               case Global.EaseType.EaseInOutSine:
                   return -(Mathf.Cos(Mathf.PI * t) - 1f) / 2f;
               case Global.EaseType.EaseInQuad:
                   return t * t;
               case Global.EaseType.EaseOutQuad:
                   return t * (2f - t);
               case Global.EaseType.EaseInOutQuad:
                   return t < 0.5f ? 2f * t * t : -1f + (4f - 2f * t) * t;
               case Global.EaseType.EaseInCubic:
                   return t * t * t;
               case Global.EaseType.EaseOutCubic:
                   return (--t) * t * t + 1f;
               case Global.EaseType.EaseInOutCubic:
                   return t < 0.5 ? 4 * t * t * t : 1 - Mathf.Pow(-2 * t + 2, 3) / 2;
               case Global.EaseType.EaseInQuart:
                   return t * t * t * t;
               case Global.EaseType.EaseOutQuart:
                   return 1 - Mathf.Pow(1 - t, 4);
               case Global.EaseType.EaseInOutQuart:
                   return t < 0.5 ? 8 * t * t * t * t : 1 - Mathf.Pow(-2 * t + 2, 4) / 2;
               case Global.EaseType.EaseInQuint:
                   return t * t * t * t * t;
               case Global.EaseType.EaseOutQuint:
                   return 1 - Mathf.Pow(1 - t, 5);
               case Global.EaseType.EaseInOutQuint:
                   return t < 0.5 ? 16 * t * t * t * t * t : 1 - Mathf.Pow(-2 * t + 2, 5) / 2;
               case Global.EaseType.EaseInExpo:
                   return t == 0 ? 0 : Mathf.Pow(2, 10 * (t - 1));
               case Global.EaseType.EaseOutExpo:
                   return t == 1 ? 1 : 1 - Mathf.Pow(2, -10 * t);
               case Global.EaseType.EaseInOutExpo:
                   return t == 0 
                            ? 0
                            : t == 1
                                ? 1
                                : t < 0.5f 
                                    ? Mathf.Pow(2,20 * t - 10) /2
                                    : (2 - Mathf.Pow(2, -20 * t + 10)) / 2;
               case Global.EaseType.EaseInCirc:
                   return 1 - Mathf.Sqrt(1 - t * t);
               case Global.EaseType.EaseOutCirc:
                   return Mathf.Sqrt(1 - Mathf.Pow(t-1,2));
               case Global.EaseType.EaseInOutCirc:
                   return t < 0.5f
                       ? (1 - Mathf.Sqrt(1 - Mathf.Pow(2 * t, 2))) / 2
                       : (Mathf.Sqrt(1 - Mathf.Pow(-2 * t + 2, 2)) + 1) / 2;
               case Global.EaseType.EaseInBack:
                   return c3 * t * t * t - c1 * t * t;
               case Global.EaseType.EaseOutBack:
                   return 1 + c3 * Mathf.Pow(t - 1, 3) + c1 * Mathf.Pow(t - 1, 2);
               case Global.EaseType.EaseInOutBack:
                   return t < 0.5f
                            ? (Mathf.Pow(2 * t, 2) * ((c2 + 1) * 2 * t - c2)) / 2
                            : (Mathf.Pow(2 * t - 2, 2) * ((c2 + 1) * (t * 2 - 2) + c2) + 2) / 2;
               case Global.EaseType.EaseInElastic:
                   return t == 0
                       ? 0
                       : t == 1
                           ? 1
                           : -Mathf.Pow(2, 10 * t - 10) * Mathf.Sin((t * 10 - 10.75f) * c4);
               case Global.EaseType.EaseOutElastic:
                   return t == 0
                            ? 0
                            : t == 1
                            ? 1
                            : Mathf.Pow(2, -10 * t) * Mathf.Sin((t * 10 - 0.75f) * c4) + 1;
               case Global.EaseType.EaseInOutElastic:
                   return t == 0
                            ? 0
                            : t == 1
                            ? 1
                            : t < 0.5f
                            ? -(Mathf.Pow(2, 20 * t - 10) * Mathf.Sin((20 * t - 11.125f) * c5)) / 2
                            : (Mathf.Pow(2, -20 * t + 10) * Mathf.Sin((20 * t - 11.125f) * c5)) / 2 + 1;
               case Global.EaseType.AnimationCurve:
                   return m_curve?.Evaluate(t) ?? t;
                default:
                   return t;
           }
       }
    }
}
