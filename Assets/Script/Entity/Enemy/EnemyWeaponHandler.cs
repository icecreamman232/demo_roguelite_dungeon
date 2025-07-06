using SGGames.Script.Weapons;
using UnityEngine;

namespace SGGames.Script.Entity
{
    public class EnemyWeaponHandler : EntityBehavior
    {
        [SerializeField] private Transform m_weaponAttachment;
        [SerializeField] private EnemyWeapon m_currWeapon;

        private void Start()
        {
            m_currWeapon.Initialize(this.gameObject);
        }
    }
}
