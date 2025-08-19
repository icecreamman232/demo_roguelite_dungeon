using TMPro;
using UnityEngine;

namespace SGGames.Script.UI
{
    public class CooldownTimerUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_cooldownText;
        private int m_currentCooldown;
        
        public void StartCooldownTimer(int cooldown)
        {
            m_currentCooldown = cooldown;
            m_cooldownText.text = cooldown.ToString();
        }

        public void UpdateCooldownTimer(int cooldown)
        {
            m_currentCooldown = cooldown;
            m_cooldownText.text = cooldown.ToString();
        }
    }
}
