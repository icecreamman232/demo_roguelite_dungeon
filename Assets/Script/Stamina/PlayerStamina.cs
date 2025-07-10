using System;
using System.Collections;
using SGGames.Script.Entity;
using SGGames.Script.Events;
using UnityEngine;

namespace SGGames.Script.StaminaSystem
{
    public class PlayerStamina : EntityBehavior
    {
        [SerializeField] private int m_maxStamina;
        [SerializeField] private int m_currentStamina;
        [SerializeField] private float m_staminaRegenDelayTime;
        [SerializeField] private UpdatePlayerStaminaEvent m_updatePlayerStaminaEvent;
        
        private bool m_isRegenerating;
        
        public int MaxStamina => m_maxStamina;
        public int CurrentStamina => m_currentStamina;
        public float StaminaRegenDelayTime => m_staminaRegenDelayTime;

        private void Start()
        {
            m_currentStamina = m_maxStamina;
            UpdateStaminaBar(isInitialize:true);
        }

        private IEnumerator RegenerateStamina()
        {
            m_isRegenerating = true;
            yield return new WaitForSeconds(m_staminaRegenDelayTime);
            m_currentStamina = m_maxStamina;
            UpdateStaminaBar();
            m_isRegenerating = false;
        }

        private void UpdateStaminaBar(bool isInitialize = false)
        {
            m_updatePlayerStaminaEvent.Raise(m_currentStamina, m_maxStamina, isInitialize);
        }
        
        public bool HaveEnoughStamina(int stamina)
        {
            return CurrentStamina >= stamina;
        }

        public bool CanUseStamina(int stamina)
        {
            return !m_isRegenerating && HaveEnoughStamina(stamina);
        }
        
        public void UseStamina(int stamina)
        {
            m_currentStamina -= stamina;
            UpdateStaminaBar();
            if (m_currentStamina <= 0)
            {
                StartCoroutine(RegenerateStamina());
            }
        }
    }
}

