using System;
using System.Collections;
using SGGames.Script.Core;
using UnityEngine;

namespace SGGames.Script.UI
{
    public class LoadingScreenController : MonoBehaviour, IGameService
    {
        [SerializeField] private bool m_isDefaultFadeOut;
        [SerializeField] private CanvasGroup m_canvasGroup;

        private bool m_isLoading = false;

        private void Awake()
        {
            ServiceLocator.RegisterService<LoadingScreenController>(this);
            m_canvasGroup.alpha = m_isDefaultFadeOut ? 1 : 0;
        }

        public void FadeOutToBlack(float duration = 0.5f)
        {
            if (m_isLoading) return;
            Debug.Log("FadeOutToBlack");
            StartCoroutine(OnFadeOut(duration));
        }

        private IEnumerator OnFadeOut(float duration)
        {
            m_isLoading = true;
            var timer = 0f;
            while (m_canvasGroup.alpha < 1)
            {
                timer += Time.unscaledDeltaTime;
                m_canvasGroup.alpha = MathHelpers.Remap(timer, 0, duration, 0, 1);
                yield return null;
            }
            m_canvasGroup.alpha = 1;
            m_isLoading = false;
        }

        public void FadeInFromBlack(float duration = 0.5f)
        {
            if (m_isLoading) return;
            Debug.Log("FadeInFromBlack");
            StartCoroutine(OnFadeIn(duration));
        }
        
        private IEnumerator OnFadeIn(float duration)
        {
            m_isLoading = true;
            var timer = duration;
            while (m_canvasGroup.alpha > 0)
            {
                timer -= Time.unscaledDeltaTime;
                m_canvasGroup.alpha = MathHelpers.Remap(timer, 0, duration, 0, 1);
                yield return null;
            }
            m_canvasGroup.alpha = 0;
            m_isLoading = false;
        }

        [ContextMenu("Fade In")]
        private void TestFadeIn()
        {
            FadeInFromBlack(2f);
        }

        [ContextMenu("Fade Out")]
        private void TestFadeOut()
        {
            FadeOutToBlack(2f);
        }
    }
}
