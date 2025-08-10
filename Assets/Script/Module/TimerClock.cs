using UnityEngine;

namespace SGGames.Script.Modules
{
    public class TimerClock
    {
        private float m_duration;
        private float m_elapsedTime;
        private bool m_isRunning;
        
        public bool IsRunning => m_isRunning;
        
        public void Start(float duration)
        {
            m_duration = duration;
            m_elapsedTime = m_duration;
            m_isRunning = true;
        }

        public void Update()
        {
            if (!m_isRunning) return;
            m_elapsedTime -= Time.deltaTime;
            if (m_elapsedTime <= 0f)
            {
                m_elapsedTime = 0;
                m_isRunning = false;
            }
        }
    }
}
