using SGGames.Script.Data;

namespace SGGames.Script.Weapons
{
    public class WeaponReadyState : IWeaponState
    {
        public void Initialize(WeaponData data) { }
        public void Enter(IWeapon weapon) { }

        public void Update(IWeapon weapon) { }

        public void Exit(IWeapon weapon) { }
    }
}

