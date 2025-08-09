using System;
using SGGames.Script.Events;
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

        private void OnReceiveStaminaUpdateEvent(int current, int max, bool isInitialized)
        {
            if (isInitialized)
            {
                m_view.SetupView(max);
            }
            else
            {
                m_view.UpdateView(current, max);
            }
        }
    }
}

