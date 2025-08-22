using System;
using System.Collections.Generic;
using UnityEngine;

namespace SGGames.Script.Weapons
{
    public class ProjectileManager
    {
        private Dictionary<string,ProjectileGroup> m_projectileGroup = new Dictionary<string, ProjectileGroup>();
        
        public void RegisterProjectileGroup(string groupID, Action onAllProjectilesStopped)
        {
            if(!m_projectileGroup.ContainsKey(groupID))
            {
                m_projectileGroup[groupID] = new ProjectileGroup(onAllProjectilesStopped);
            }
        }
        
        public void AddProjectile(string groupID, Projectile projectile)
        {
            if (m_projectileGroup.TryGetValue(groupID, out var group))
            {
                group.AddProjectile(projectile);
                projectile.OnProjectileStopped = ()=> OnProjectileStopped(groupID, projectile);
            }
        }
        
        public void ClearGroup(string groupID)
        {
            if (m_projectileGroup.TryGetValue(groupID, out var group))
            {
                group.Clear();
                m_projectileGroup.Remove(groupID);
            }
        }

        public int GetActiveProjectileCount(string groupID)
        {
            return m_projectileGroup.TryGetValue(groupID, out var group) ? group.Count : 0;
        }

        public bool HasActiveProjectile(string groupID)
        {
            return GetActiveProjectileCount(groupID) > 0;
        }

        private void OnProjectileStopped(string groupID, Projectile projectile)
        {
            if (m_projectileGroup.TryGetValue(groupID, out var group))
            {
                group.RemoveProjectile(projectile);
                if (group.IsEmpty)
                {
                    group.OnAllProjectilesStopped?.Invoke();
                    ClearGroup(groupID);
                }
            }
        }
    }
}
