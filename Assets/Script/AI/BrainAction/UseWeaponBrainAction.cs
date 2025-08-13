using SGGames.Script.Core;
using SGGames.Script.Entity;
using SGGames.Scripts.AI;
using SGGames.Scripts.Entity;

namespace SGGames.Script.AI
{
    public class UseWeaponBrainAction : BrainAction
    {
        private EnemyWeaponHandler m_enemyWeaponHandler;

        public override void Initialize(EnemyBrain brain)
        {
            m_enemyWeaponHandler = brain.Owner.WeaponHandler;
            base.Initialize(brain);
        }

        public override void StartTurnAction()
        {
            m_enemyWeaponHandler.UseWeapon();
            SetActionState(Global.ActionState.InProgress);
        }

        public override void UpdateAction()
        {
            if (!m_enemyWeaponHandler.IsAttacking)
            {
                SetActionState(Global.ActionState.Completed);
            }
            base.UpdateAction();
        }
    }
}
