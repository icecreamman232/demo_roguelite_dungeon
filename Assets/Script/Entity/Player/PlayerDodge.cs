using SGGames.Scripts.Core;
using SGGames.Scripts.Data;
using SGGames.Scripts.Items;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SGGames.Scripts.Entities
{
    public class PlayerDodge : EntityBehavior
    {
        [SerializeField] private WorldEvent m_worldEvent;
        [SerializeField] private PlayerData m_data;
        [SerializeField] private float m_currentDodge; //Percentage
        
        private void Start()
        {
            m_currentDodge = m_data.DefaultDodgeChance;
        }
        
        public void AddDodge(float dodge)
        {
            m_currentDodge += dodge;
            m_currentDodge = Mathf.Clamp(m_currentDodge, 0, m_data.MaxDodgeChance);
        }

        public void RemoveDodge(float dodge)
        {
            m_currentDodge -= dodge;
            m_currentDodge = Mathf.Clamp(m_currentDodge, 0, m_data.MaxDodgeChance);       
        }

        public bool CanDodgeThisAttack()
        {
            var randomPercentage = Random.Range(0, 100);
            var canDodge = randomPercentage <= m_currentDodge;
            if (canDodge)
            {
                m_worldEvent.Raise(Global.WorldEventType.OnPlayerPerfectDodge,this.gameObject, null);
            }
            return canDodge;
        }
    }
}
