using SGGames.Script.Data;
using SGGames.Script.Entity;
using UnityEngine;

namespace SGGames.Script.Entities
{
    public class PlayerResistanceController : EntityBehavior
    {
        [SerializeField] private PlayerData m_data; 
        [SerializeField] private float m_currentDamageResistance; //Percentage
        
        public float CurrentDamageResistance => m_currentDamageResistance;
        
        public void Initialize()
        {
            m_currentDamageResistance = m_data.DefaultDamageResistance;
        }
        
        public void AddDamageResistance(float damageResistance)
        {
            m_currentDamageResistance += damageResistance;
            m_currentDamageResistance = Mathf.Clamp(m_currentDamageResistance, 0, m_data.MaxDamageResistance);
        }
    }
}
