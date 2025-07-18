using SGGames.Script.Core;
using SGGames.Script.Data;
using UnityEngine;

namespace SGGames.Script.Weapons
{
    public class WeaponCoolDownState : IWeaponState
    {
        private float m_cooldown;
        private float m_timer;
        
        public void Initialize(WeaponData data)
        {
            m_cooldown = data.Cooldown;
        }

        public void Enter(IWeapon weapon)
        {
            m_timer = m_cooldown;
        }

        public void Update(IWeapon weapon)
        {
            m_timer -= Time.deltaTime;
            if (m_timer <= 0)
            {
                m_timer = 0;
                weapon.ChangeState(Global.WeaponState.Ready);
            }
        }

        public void Exit(IWeapon weapon)
        {
            
        }
    }
}

