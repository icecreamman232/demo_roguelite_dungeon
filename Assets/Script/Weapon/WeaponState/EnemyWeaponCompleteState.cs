
using SGGames.Script.Core;
using SGGames.Script.Modules;

namespace SGGames.Script.Weapons
{
    public class EnemyWeaponCompleteState : IWeaponState
    {
        private IStateTransitioner<Global.WeaponState> m_transitioner;
        
        public EnemyWeaponCompleteState(IStateTransitioner<Global.WeaponState> transitioner)
        {
            m_transitioner = transitioner;
        }
        
        public void Enter(Weapon weapon)
        {
            m_transitioner.ChangeState(Global.WeaponState.Ready);
        }

        public void Update(Weapon weapon)
        {
            
        }

        public void Exit(Weapon weapon)
        {
            
        }
    }
}

