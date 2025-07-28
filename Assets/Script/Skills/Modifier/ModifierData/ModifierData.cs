using UnityEngine;

namespace SGGames.Script.Skills
{
    public abstract class ModifierData : ScriptableObject
    {
        [SerializeField] protected float m_duration;
        
        public float Duration => m_duration;
    }
}
