
namespace SGGames.Scripts.Modules
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

        public void Update(float deltaTime)
        {
            if (!m_isRunning) return;
            m_elapsedTime -= deltaTime;
            if (m_elapsedTime <= 0f)
            {
                m_elapsedTime = 0;
                m_isRunning = false;
            }
        }
    }
}
