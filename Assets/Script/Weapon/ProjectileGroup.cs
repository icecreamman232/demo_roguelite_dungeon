using System;
using System.Collections.Generic;

namespace SGGames.Script.Weapons
{
    public class ProjectileGroup
    {
        private HashSet<Projectile> m_projectiles = new HashSet<Projectile>();
        public Action OnAllProjectilesStopped { get; }
        public int Count => m_projectiles.Count;
        public bool IsEmpty => m_projectiles.Count == 0;

        public  ProjectileGroup(Action onAllProjectilesStopped)
        {
            OnAllProjectilesStopped = onAllProjectilesStopped;
        }
        
        public void AddProjectile(Projectile projectile)
        {
            m_projectiles.Add(projectile);
        }
        
        public void RemoveProjectile(Projectile projectile)
        {
            m_projectiles.Remove(projectile);
        }
        
        public void Clear()
        {
            m_projectiles.Clear();
        }
    }
}
