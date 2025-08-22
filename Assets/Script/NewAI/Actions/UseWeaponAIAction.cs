using SGGames.Scripts.Core;

namespace SGGames.Scripts.AI
{
    public class UseWeaponAIAction : AIAction
    {
        public override void DoAction()
        {
            m_brain.Owner.WeaponHandler.UseWeapon();
            SetActionState(Global.ActionState.InProgress);
            base.DoAction();
        }

        public override void UpdateAction()
        {
            if (!m_brain.Owner.WeaponHandler.IsAttacking)
            {
                SetActionState(Global.ActionState.Completed);
            }
            base.UpdateAction();
        }
    }
}

