
namespace SGGames.Scripts.Weapons
{
    public interface IProjectileSpawner
    {
        void InitializeProjectileSpawner(ProjectileBuilder builder);
        void SpawnProjectile();
    }
}
