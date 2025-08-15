using SGGames.Script.Core;
using SGGames.Script.Modules;

namespace SGGames.Script.Weapons
{
    public class EnemyWeaponStateMachine : FiniteStateMachine<Weapon, Global.WeaponState>
    {
        public EnemyWeaponStateMachine(Weapon context, (Global.WeaponState, IState<Weapon>)[] states) : base(context, states)
        {
        }
    }
}
