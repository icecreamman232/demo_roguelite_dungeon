using UnityEngine;
using UnityEngine.UI;

namespace SGGames.Script.UI
{
    public class StaminaUISlot : MonoBehaviour
    {
        [SerializeField] private Sprite m_fullSprite;
        [SerializeField] private Sprite m_emptySprite;
        [SerializeField] private Image m_slotImage;

        public void UpdateSlot(bool isFull)
        {
            m_slotImage.sprite = isFull ? m_fullSprite : m_emptySprite;
        }
    }
}
