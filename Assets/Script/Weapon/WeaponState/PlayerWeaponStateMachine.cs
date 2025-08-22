using SGGames.Scripts.Core;
using SGGames.Scripts.Modules;

namespace SGGames.Scripts.Weapons
{
    public class PlayerWeaponStateMachine : FiniteStateMachine<Weapon, Global.WeaponState>
    {
        public PlayerWeaponStateMachine(Weapon context, (Global.WeaponState, IState<Weapon>)[] states) : base(context, states) { }
    }
}
