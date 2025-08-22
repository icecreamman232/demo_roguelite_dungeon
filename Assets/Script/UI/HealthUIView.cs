using SGGames.Scripts.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SGGames.Scrips.UI
{
    public class HealthUIView : UIView
    {
        [SerializeField] private Image m_healthBar;
        [SerializeField] private TextMeshProUGUI m_healthText;
        
        public void UpdateHealthBar(float currentHealth, float maxHealth)
        {
            m_healthBar.fillAmount = MathHelpers.Remap(currentHealth, 0, maxHealth, 0, 1);
            m_healthText.text = $"{currentHealth}/{maxHealth}";
        }
    }
}

