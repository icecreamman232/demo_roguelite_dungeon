using SGGames.Scripts.Core;
using TMPro;
using UnityEngine;

namespace SGGames.Scripts.Modules
{
    public class FloatingText : MonoBehaviour
    {
        [SerializeField] private bool m_haveOffsetX;
        [SerializeField] private AnimationCurve m_xOffsetOverTime;
        [SerializeField] private bool m_haveOffsetY;
        [SerializeField] private AnimationCurve m_yOffsetOverTime;
        [SerializeField] private TextMeshPro m_text;
        private float m_lifeTime;
        private bool m_isAlive;
        private float m_timer;
        private Vector3 m_startPosition;

        private void OnEnable()
        {
            m_timer = 0;
        }

        private void Update()
        {
            if (!m_isAlive) return;

            m_timer += Time.deltaTime;
            
            var offsetX = m_haveOffsetX ? m_xOffsetOverTime.Evaluate(MathHelpers.Remap(m_timer,0,m_lifeTime,0,1)) : 0;
            var offsetY = m_haveOffsetY ? m_yOffsetOverTime.Evaluate(MathHelpers.Remap(m_timer,0,m_lifeTime,0,1)) : 0;
            
            transform.position = m_startPosition + new Vector3(offsetX, offsetY, 0);
            
            if (m_timer >= m_lifeTime)
            {
                Kill();
            }
        }

        private void Kill()
        {
            m_isAlive = false;
            gameObject.SetActive(false);
        }

        public void SetupFloatingText(string content, Vector3 position, float lifeTime)
        {
            m_text.text = content;
            m_startPosition = position;
            transform.position = position;
            m_lifeTime = lifeTime;
            m_isAlive = true;
        }
    }
}
