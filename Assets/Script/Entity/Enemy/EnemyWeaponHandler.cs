using SGGames.Script.Weapons;
using SGGames.Scripts.Entity;
using UnityEngine;

namespace SGGames.Script.Entity
{
    public class EnemyWeaponHandler : EntityBehavior, IWeaponOwner
    {
        [SerializeField] private Weapon m_currWeapon;
        
        private EnemyController m_controller;
        public Weapon CurrentWeapon => m_currWeapon;
        public EnemyController Controller => m_controller;

        public bool IsAttacking => m_currWeapon.IsAttacking();
 

        public void Initialize(EnemyController controller)
        {
            m_controller = controller;
            m_currWeapon.InitializeWeapon(this);
        }
        
        public void UseWeapon()
        {
            m_currWeapon.Attack();
        }
    }
}
