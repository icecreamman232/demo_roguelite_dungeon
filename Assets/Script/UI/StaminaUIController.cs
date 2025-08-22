using SGGames.Scripts.Events;
using UnityEngine;

namespace SGGames.Scripts.UI
{
    public class StaminaUIController : UIController
    {
        [SerializeField] private StaminaUIView m_view;
        [SerializeField] private UpdatePlayerStaminaEvent m_updatePlayerStaminaEvent;

        private void Awake()
        {
            m_updatePlayerStaminaEvent.AddListener(OnReceiveStaminaUpdateEvent);
        }

        private void OnDestroy()
        {
            m_updatePlayerStaminaEvent.RemoveListener(OnReceiveStaminaUpdateEvent);
        }

        private void OnReceiveStaminaUpdateEvent(UpdatePlayerStaminaEventData updatePlayerStaminaEventData)
        {
            if (updatePlayerStaminaEventData.IsInitialize)
            {
                m_view.SetupView(updatePlayerStaminaEventData.MaxStamina);
            }
            else
            {
                m_view.UpdateView(updatePlayerStaminaEventData.CurrentStamina, updatePlayerStaminaEventData.MaxStamina);
            }
        }
    }
}

