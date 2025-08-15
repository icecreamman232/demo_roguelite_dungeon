using SGGames.Script.Core;
using SGGames.Script.Modules;


namespace SGGames.Script.Weapons
{
    public class PlayerWeaponStateMachine : FiniteStateMachine<Weapon, Global.WeaponState>
    {
        public PlayerWeaponStateMachine(Weapon context, (Global.WeaponState, IState<Weapon>)[] states) : base(context, states)
        {
        }
    }
}
