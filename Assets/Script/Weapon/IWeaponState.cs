
using SGGames.Script.Data;

namespace SGGames.Script.Weapons
{
    public interface IWeaponState
    {
        void Initialize(WeaponData data);
        void Enter(IWeapon weapon);
        void Update(IWeapon weapon);
        void Exit(IWeapon weapon);
    }
}
