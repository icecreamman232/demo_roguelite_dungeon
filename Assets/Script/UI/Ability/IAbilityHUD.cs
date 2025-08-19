
using System;

namespace SGGames.Script.UI
{
    public interface IAbilityHUD
    {
        void ShowAbilityButton();
        void ShowExecuteButtons();
        void ShowCooldown(int cooldown);
    }
}
