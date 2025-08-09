
using System;
using SGGames.Script.Events;
using UnityEngine;

namespace SGGames.Script.HealthSystem
{
    public class PlayerDamageHandler : DamageHandler
    {
        [SerializeField] private float m_flatDamageModifier;
        [SerializeField] private UpdatePlayerProjectileDamageEvent m_playerProjectileDamageEvent;

        private void Awake()
        {
            m_playerProjectileDamageEvent.AddListener(OnReceiveDamageEvent);
        }

        private void OnDestroy()
        {
            m_playerProjectileDamageEvent.RemoveListener(OnReceiveDamageEvent);
        }

        private void OnReceiveDamageEvent(float modifierValue)
        {
            m_flatDamageModifier += modifierValue;
        }

        protected override float GetDamage()
        {
            m_minDamage += m_flatDamageModifier;
            m_maxDamage += m_flatDamageModifier;
            return base.GetDamage();
        }
    }
}
