using SGGames.Scripts.Core;
using SGGames.Scripts.Modules;

namespace SGGames.Scripts.Weapons
{
    public class EnemyWeaponStateMachine : FiniteStateMachine<Weapon, Global.WeaponState>
    {
        public EnemyWeaponStateMachine(Weapon context, (Global.WeaponState, IState<Weapon>)[] states) : base(context, states) { }
    }
}
