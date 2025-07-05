using System.Collections;
using SGGames.Script.Core;
using SGGames.Script.Data;
using UnityEngine;

namespace SGGames.Script.Weapons
{
    /// <summary>
    /// Base class for all weapons
    /// </summary>
    public class Weapon : MonoBehaviour
    {
        [SerializeField] protected GameObject m_owner;
        [SerializeField] protected Global.WeaponState m_currWeaponState;
        [SerializeField] protected WeaponData m_weaponData;
        [SerializeField] protected ObjectPooler m_projectilePooler;

        public virtual void Initialize(GameObject owner)
        {
            m_owner = owner;
        }
        
        protected virtual void OnAttack()
        {
            if (m_currWeaponState != Global.WeaponState.Ready) return;
            SpawnProjectile();
            OnCoolDown();
            UpdateAnimatorOnAttack();
            SetWeaponState(Global.WeaponState.CoolDown);
        }

        protected virtual void SetWeaponState(Global.WeaponState newState)
        {
            m_currWeaponState = newState;
        }

        protected virtual void SpawnProjectile()
        {
            var projectileGO = m_projectilePooler.GetPooledGameObject();
            var projectile = projectileGO.GetComponent<Projectile>();
            projectile.Spawn(new ProjectileBuilder()
                .SetPosition(transform.position)
                .SetOwner(m_owner));
        }

        protected virtual void OnCoolDown()
        {
            StartCoroutine(CoolDownCoroutine());
        }

        protected virtual IEnumerator CoolDownCoroutine()
        {
            yield return new WaitForSeconds(m_weaponData.Cooldown);
            SetWeaponState(Global.WeaponState.Ready);
        }
        
        protected virtual void UpdateAnimatorOnAttack()
        {
            
        }
    }
}
