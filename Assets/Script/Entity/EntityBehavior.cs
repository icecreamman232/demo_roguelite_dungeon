using UnityEngine;

namespace SGGames.Scripts.Entities
{
    /// <summary>
    /// Base class for all entity behavior. This is used to enable/disable any entity behavior
    /// </summary>
    public class EntityBehavior : MonoBehaviour
    {
        /// <summary>
        /// Behavior permission. Enable means this behavior is allowed to process
        /// </summary>
        [Header("Entity")]
        [SerializeField] protected bool m_isPermit;
        
        public bool IsPermit => m_isPermit;

        public void SetPermission(bool isPermit)
        {
            m_isPermit = isPermit;
        }

        protected virtual void OnGamePaused()
        {
            
        }

        protected virtual void OnGameResumed()
        {
            
        }
    }
}
