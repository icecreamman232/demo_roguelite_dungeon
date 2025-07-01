using UnityEngine;

namespace SGGames.Script.Managers
{
    [CreateAssetMenu(fileName = "Debug Settings", menuName = "SGGames/Debug Settings")]
    public class DebugSettings : ScriptableObject
    {
        [Header("General")]
        [SerializeField] private bool m_showDebug;
        [SerializeField] private bool m_showPlayerDamageDebug;
        [SerializeField] private bool m_showEnemyDamageDebug;
        [Header("Player")]
        [SerializeField] private bool m_isPlayerImmortal;

        private void Awake()
        {
            #if DEBUG
            m_showDebug = true;
            #else
            m_showDebug = false;
            #endif
        }

        public bool ShowPlayerDamageDebug => m_showDebug && m_showPlayerDamageDebug;
        public bool ShowEnemyDamageDebug => m_showDebug && m_showEnemyDamageDebug;
        
        public bool IsPlayerImmortal => m_isPlayerImmortal;


        public void ResetSettings()
        {
            m_isPlayerImmortal = false;
        }
    }
}
