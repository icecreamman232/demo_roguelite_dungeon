using System;
using UnityEngine;

namespace SGGames.Script.Managers
{
    [CreateAssetMenu(fileName = "Debug Settings", menuName = "SGGames/Debug Settings")]
    public class DebugSettings : ScriptableObject
    {
        [SerializeField] private bool m_showDebug;
        [SerializeField] private bool m_showPlayerDamageDebug;
        [SerializeField] private bool m_showEnemyDamageDebug;

        private void Awake()
        {
            #if !UNITY_EDITOR
            m_showDebug = false;
            #endif
        }

        public bool ShowPlayerDamageDebug => m_showDebug && m_showPlayerDamageDebug;
        public bool ShowEnemyDamageDebug => m_showDebug && m_showEnemyDamageDebug;
    }
}
