
using SGGames.Script.Data;

namespace SGGames.Script.Weapons
{
    public interface IWeaponState
    {
        void Initialize(WeaponData data);
        void Enter(Weapon weapon);
        void Update(Weapon weapon);
        void Exit(Weapon weapon);
    }
}
