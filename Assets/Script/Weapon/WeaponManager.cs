using System;
using System.Collections.Generic;
using System.Linq;
using SGGames.Script.Core;
using SGGames.Script.Modules;
using UnityEngine;

namespace SGGames.Script.Weapons
{
    public class WeaponManager
    {
        private List<Weapon> m_weapons;
        private Global.WeaponSelectionStrategy m_strategy;
        private int m_currentWeaponIndex;
        private IWeaponOwner m_owner;
        private ProjectileManager m_projectileManager;

        public Weapon CurrentWeapon => m_weapons?.ElementAtOrDefault(m_currentWeaponIndex);

        public WeaponManager(List<Weapon> weapons, Global.WeaponSelectionStrategy strategy, ProjectileManager projectileManager)
        {
            m_weapons = weapons;
            m_strategy = strategy;
            m_projectileManager = projectileManager;
            m_currentWeaponIndex = 0;
        }

        public void Initialize(IWeaponOwner owner)
        {
            m_owner = owner;
            foreach (var weapon in m_weapons)
            {
                weapon.InitializeWeapon(owner);
                if (weapon is IProjectileTrackable trackableWeapon)
                {
                    trackableWeapon.SetProjectileManager(m_projectileManager);
                }
            }
        }

        public bool IsAttacking()
        {
            return m_strategy switch
            {
                Global.WeaponSelectionStrategy.Simultaneous => m_weapons.Any(w => w.IsAttacking()),
                _ => CurrentWeapon?.IsAttacking() ?? false
            };
        }

        public void UseCurrentWeapon(string attackGroupID)
        {
            switch (m_strategy)
            {
                case Global.WeaponSelectionStrategy.Simultaneous:
                    UseAllWeapons(attackGroupID);
                    break;
                default:
                    UseWeapon(CurrentWeapon, attackGroupID);
                    break;
            }
        }
        
        public void SwitchToNextWeapon()
        {
            if (m_weapons.Count <= 1) return;

            switch (m_strategy)
            {
                case Global.WeaponSelectionStrategy.Sequential:
                    m_currentWeaponIndex = (m_currentWeaponIndex + 1) % m_weapons.Count;
                    break;
            }
        }
        
        public void SwitchToWeapon(int index)
        {
            if (index >= 0 && index < m_weapons.Count)
            {
                m_currentWeaponIndex = index;
            }
        }

        public void ChangeWeaponState(Global.WeaponState newState)
        {
            switch (m_strategy)
            {
                case Global.WeaponSelectionStrategy.Simultaneous:
                    foreach (var weapon in m_weapons)
                    {
                        if (weapon is IStateTransitioner<Global.WeaponState> transitioner)
                        {
                            transitioner.ChangeState(newState);
                        }
                    }
                    break;
                case Global.WeaponSelectionStrategy.Sequential:
                    if (CurrentWeapon != null && CurrentWeapon is IStateTransitioner<Global.WeaponState> currentTransitioner)
                    {
                        currentTransitioner.ChangeState(newState);
                    }
                    break;
            }
        }

        
        private void UseAllWeapons(string attackGroupID)
        {
            foreach (var weapon in m_weapons)
            {
                UseWeapon(weapon, attackGroupID);
            }
        }
        
        private void UseWeapon(Weapon weapon, string attackGroupId)
        {
            if (weapon is IProjectileTrackable trackableWeapon)
            {
                trackableWeapon.AttackWithTracking(attackGroupId);
            }
            else
            {
                weapon.Attack();
            }
        }

    }
}
