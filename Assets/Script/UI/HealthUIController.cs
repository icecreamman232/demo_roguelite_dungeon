using SGGames.Script.Core;
using SGGames.Script.Events;
using SGGames.Scripts.UI;
using UnityEngine;


namespace SGGames.Script.UI
{
    public class HealthUIController : UIController
    {
        [SerializeField] private HealthUIView m_view;
        [SerializeField] private RectTransform m_healthBarPivot;
        [SerializeField] private HealthUISlot m_healthUISlotPrefab;
        [SerializeField] private UpdatePlayerHealthEvent m_updatePlayerHealthEvent;
        
        private void Awake()
        {
            m_updatePlayerHealthEvent.AddListener(OnReceiveUpdateHealthEvent);
        }

        private void Initialize(float currentHealth, float maxHealth)
        {
            var slotNumber = currentHealth / Global.HP_PER_SLOT;
            for (int i = 0; i < slotNumber; i++)
            {
                var slotController = Instantiate(m_healthUISlotPrefab, m_healthBarPivot);
                slotController.Setup(i, Global.HealthSlotType.Health);
                m_view.AddSlotToView(slotController);
            }
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

        private void OnDestroy()
        {
            m_updatePlayerHealthEvent.RemoveListener(OnReceiveUpdateHealthEvent);
        }
    }
}
