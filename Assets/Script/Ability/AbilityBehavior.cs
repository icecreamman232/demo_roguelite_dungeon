using UnityEngine;

namespace SGGames.Scripts.Abilities
{
    public class AbilityBehavior : MonoBehaviour
    {
        [SerializeField] protected bool m_isPermit;
        [SerializeField] protected bool m_isDefaultActivated;
        [SerializeField] protected bool m_isActivated;
        public bool IsPermit => m_isPermit;
        public bool IsActivated => m_isActivated;
        public bool IsDefaultActivated => m_isDefaultActivated;
        
        public virtual void SetPermission(bool isPermit)
        {
            m_isPermit = isPermit;
        }

        public virtual void Activate()
        {
            m_isActivated = true;
        }

        public virtual void Deactivate()
        {
            m_isActivated = false;
        }
    }
}
