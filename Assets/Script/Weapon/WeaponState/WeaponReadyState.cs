
namespace SGGames.Scripts.Weapons
{
    public class WeaponReadyState : IWeaponState
    {
        public void Enter(Weapon weapon)
        {
            if (weapon is EnemyRangeWeapon rangeWeapon)
            {
                rangeWeapon.NumberSpawnedProjectile = 0;
            }
        }

        public void Update(Weapon weapon) { }
        public void Exit(Weapon weapon) { }
    }
}

