using SGGames.Script.Weapons;
using UnityEngine;

namespace SGGames.Script.Entity
{
    public class EnemyWeaponHandler : EntityBehavior, IWeaponOwner
    {
        [SerializeField] private Transform m_weaponAttachment;
        [SerializeField] private EnemyDefaultRangeWeapon m_currWeapon;

        private void Start()
        {
            m_currWeapon.InitializeWeapon(this);
        }
    }
}
