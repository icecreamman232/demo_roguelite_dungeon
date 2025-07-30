using SGGames.Script.Events;
using SGGames.Scripts.UI;
using UnityEngine;


namespace SGGames.Script.UI
{
    public class HealthUIController : UIController
    {
        [SerializeField] private HealthUIView m_view;
        [SerializeField] private UpdatePlayerHealthEvent m_updatePlayerHealthEvent;
        
        private void Awake()
        {
            m_updatePlayerHealthEvent.AddListener(OnReceiveUpdateHealthEvent);
        }
        
        private void OnDestroy()
        {
            m_updatePlayerHealthEvent.RemoveListener(OnReceiveUpdateHealthEvent);
        }

        private void Initialize(float currentHealth, float maxHealth)
        {
            m_view.UpdateHealthBar(currentHealth, maxHealth);
        }

        private void OnReceiveUpdateHealthEvent(float currentHealth, float maxHealth, bool isInitialize)
        {
            if (isInitialize)
            {
                Initialize(currentHealth, maxHealth);
            }
            else
            {
                UpdateHealthUI(currentHealth, maxHealth);
            }
        }

        private void UpdateHealthUI(float currentHealth, float maxHealth)
        {
            m_view.UpdateHealthBar(currentHealth, maxHealth);
        }
    }
}
