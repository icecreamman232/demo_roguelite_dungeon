using TMPro;
using UnityEngine;

namespace SGGames.Script.UI
{
    public class CooldownTimerUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_cooldownText;
        
        public void UpdateCooldownTimer(int cooldown)
        {
            if (this.gameObject.activeSelf == false && cooldown > 0)
            {
                this.gameObject.SetActive(true);
            }

            m_cooldownText.text = cooldown.ToString();

            if (cooldown <= 0)
            {
                this.gameObject.SetActive(false);
            }
        }
    }
}
