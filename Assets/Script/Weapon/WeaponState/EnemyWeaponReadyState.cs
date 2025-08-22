using SGGames.Scripts.Data;

namespace SGGames.Scripts.Weapons
{
    public class EnemyWeaponReadyState : IWeaponState
    {
        public void Initialize(WeaponData data) { }
        public void Enter(Weapon weapon)
        {
            if (weapon is EnemyRangeWeapon rangeWeapon)
            {
                rangeWeapon.NumberSpawnedProjectile = 0;
            }
        }

        public void Update(Weapon weapon)
        {
            
        }

        public void Exit(Weapon weapon)
        {
            
        }
    }
}