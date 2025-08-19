using SGGames.Script.Core;
using UnityEngine;

namespace SGGames.Script.UI
{
    public class DashHUD : MonoBehaviour, IAbilityHUD
    {
        [SerializeField] private CanvasGroup m_abilityButtonCG;
        [SerializeField] private CanvasGroup m_executeButtonCG;
        [SerializeField] private CooldownTimerUI m_cooldownTimerUI;
        
        public void ShowAbilityButton()
        {
            m_abilityButtonCG.Activate();
            m_executeButtonCG.Deactivate();
        }

        public void ShowExecuteButtons()
        {
            m_abilityButtonCG.Deactivate();
            m_executeButtonCG.Activate();
        }

        public void ShowCooldown(int turnAmount)
        {
            m_cooldownTimerUI.UpdateCooldownTimer(turnAmount);
        }
    }
}
