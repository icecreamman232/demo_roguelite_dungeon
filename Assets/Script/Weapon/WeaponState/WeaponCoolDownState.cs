using SGGames.Scripts.Data;
using UnityEngine;

namespace SGGames.Scripts.Weapons
{
    public class WeaponCoolDownState : IWeaponState
    {
        private float m_cooldown;
        private float m_timer;
        
        public void Initialize(WeaponData data)
        {
            m_cooldown = data.Cooldown;
        }

        public void Enter(Weapon weapon)
        {
            m_timer = m_cooldown;
        }

        public void Update(Weapon weapon)
        {
            m_timer -= Time.deltaTime;
            if (m_timer <= 0)
            {
                m_timer = 0;
                //weapon.ChangeState(Global.WeaponState.Ready);
            }
        }

        public void Exit(Weapon weapon) { }
    }
}

