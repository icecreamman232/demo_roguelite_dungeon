using UnityEngine;

namespace SGGames.Script.Items
{
    public abstract class ModifierData : ScriptableObject
    {
        [SerializeField] protected float m_duration;
        
        public float Duration => m_duration;
    }
}
