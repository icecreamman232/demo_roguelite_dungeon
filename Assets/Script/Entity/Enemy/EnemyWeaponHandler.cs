using System.Collections.Generic;
using SGGames.Script.Core;
using SGGames.Script.Weapons;
using SGGames.Scripts.Entity;
using UnityEngine;

namespace SGGames.Script.Entity
{
    public class EnemyWeaponHandler : EntityBehavior, IWeaponOwner
    {
        [SerializeField] private List<Weapon> m_availableWeapons;

        [SerializeField]
        private Global.WeaponSelectionStrategy m_selectionStrategy = Global.WeaponSelectionStrategy.Sequential;
        
        private EnemyController m_controller;
        private WeaponManager m_weaponManager;
        private ProjectileManager m_projectileManager;
        private string m_attackGroupID;
        public Weapon CurrentWeapon => m_weaponManager?.CurrentWeapon;
        public EnemyController Controller => m_controller;

        public bool IsAttacking => m_weaponManager?.IsAttacking() ?? false;
        public bool HasActiveProjectiles => m_projectileManager?.HasActiveProjectile(m_attackGroupID) ?? false;
 

        public void Initialize(EnemyController controller)
        {
            m_controller = controller;
            m_projectileManager = new ProjectileManager();
            m_weaponManager = new WeaponManager(m_availableWeapons, m_selectionStrategy, m_projectileManager);
            m_weaponManager.Initialize(this);
            
            //Create a unique attack group id for this enemy
            m_attackGroupID = $"Enemy_{GetInstanceID()}_{Time.time}";
        }
        
        public void UseWeapon()
        {
            m_projectileManager.RegisterProjectileGroup(m_attackGroupID, OnAllProjectilesFromAttackStopped);
            m_weaponManager?.UseCurrentWeapon(m_attackGroupID);
        }
        
        private void OnAllProjectilesFromAttackStopped()
        {
            // Handle logic when all projectiles from the current attack have stopped
            Debug.Log($"All projectiles from attack group {m_attackGroupID} have stopped");
            // Generate new group ID for next attack
            m_weaponManager.ChangeWeaponState(Global.WeaponState.Complete);
            m_attackGroupID = $"Enemy_{GetInstanceID()}_{Time.time}";
        }
        
        public int GetActiveProjectileCount()
        {
            return m_projectileManager?.GetActiveProjectileCount(m_attackGroupID) ?? 0;
        }

    }
}
