using System;
using SGGames.Script.Core;
using UnityEngine;
using UnityEngine.UI;

namespace SGGames.Script.UI
{
    public class HealthUISlot : MonoBehaviour
    {
        [SerializeField] private int m_slotIndex;
        [SerializeField] private Global.HealthSlotType m_slotType;
        [SerializeField] private Image m_slotImage;
        [SerializeField] private Sprite m_heartSprite;
        [SerializeField] private Sprite m_emptyHeartSprite;
        [SerializeField] private Sprite m_shieldSprite;
        [SerializeField] private Sprite m_emptyShieldSprite;

        public Global.HealthSlotType SlotType => m_slotType;
        
        public void Setup(int index,Global.HealthSlotType slotType)
        {
            m_slotIndex = index;
            m_slotType = slotType;
            SetSlotType(slotType);
        }

        public void SetSlotType(Global.HealthSlotType slotType)
        {
            switch (slotType)
            {
                case Global.HealthSlotType.Health:
                    m_slotImage.sprite = m_heartSprite;
                    break;
                case Global.HealthSlotType.HealthDisable:
                    m_slotImage.sprite = m_emptyHeartSprite;
                    break;
                case Global.HealthSlotType.Shield:
                    m_slotImage.sprite = m_shieldSprite;
                    break;
                case Global.HealthSlotType.ShieldDisable:
                    m_slotImage.sprite = m_emptyShieldSprite;
                    break;
            }
        }
    }
}

