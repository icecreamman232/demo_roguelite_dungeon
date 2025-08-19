using UnityEngine;

namespace SGGames.Script.UI
{
    public class AbilityHUDView : MonoBehaviour
    {
        private IAbilityHUD m_abilityHUD;

        public void Initialize()
        {
            m_abilityHUD = GetComponentInChildren<IAbilityHUD>();
        }
        
        public void ShowDefaultView()
        {
            m_abilityHUD.ShowAbilityButton();
        }
        
        public void ShowExecuteButtons()
        {
            m_abilityHUD.ShowExecuteButtons();
        }

        public void ShowCooldown()
        {
            
        }
    }

}
