using System.Collections;
using SGGames.Scripts.Core;
using UnityEngine;

namespace SGGames.Scripts.Modules
{
    public class PlayerCloneSprite : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer m_spriteRenderer;
        private static float S_LIVE_TIME = 0.2f;
        
        public void StartWith(Sprite sprite, bool isFlipped)
        {
            var color = m_spriteRenderer.color;
            color.a = 1;
            m_spriteRenderer.color = color;
            m_spriteRenderer.sprite = sprite;
            m_spriteRenderer.flipX = isFlipped;
            StartCoroutine(OnPlaying());
        }

        private IEnumerator OnPlaying()
        {
            var timeStop = Time.time + S_LIVE_TIME;
            var startTime = Time.time;
            
            while (Time.time < timeStop)
            {
                var timePassed = Time.time - startTime;
                var color = m_spriteRenderer.color;
                color.a = 1f - MathHelpers.Remap(timePassed, 0, S_LIVE_TIME, 0, 1);
                m_spriteRenderer.color = color;
                yield return null;
            }
            this.gameObject.SetActive(false);
        }
    }
}
