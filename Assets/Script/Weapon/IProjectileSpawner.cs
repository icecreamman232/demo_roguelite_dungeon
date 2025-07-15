using UnityEngine;

namespace SGGames.Script.Weapons
{
    public interface IProjectileSpawner
    {
        void InitializeProjectileSpawner(ProjectileBuilder builder);
        void SpawnProjectile();
    }
}
