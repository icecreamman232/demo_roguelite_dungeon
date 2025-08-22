
namespace SGGames.Scripts.Weapons
{
    /// <summary>
    ///  Interface for weapons that can track their projectiles
    /// </summary>
    public interface IProjectileTrackable
    {
        void SetProjectileManager(ProjectileManager projectileManager);
        void AttackWithTracking(string groupId);

    }
}
