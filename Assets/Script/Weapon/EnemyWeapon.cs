
using SGGames.Scripts.Entity;
using UnityEngine;

namespace SGGames.Script.Weapons
{
    public class EnemyWeapon : Weapon
    {
        private EnemyController m_controller;

        public override void Initialize(GameObject owner)
        {
            m_controller = owner.GetComponent<EnemyController>();
            base.Initialize(owner);
        }

        /// <summary>
        /// Public version of OnAttack, this means to be used by Enemy Brain
        /// </summary>
        public void Attack()
        {
            OnAttack();
        }

        protected override void SpawnProjectile()
        {
            var target = m_controller.CurrentBrain.Target;
            var numberProjectile = m_weaponData.ProjectilePerShot;

            for (int i = 0; i < numberProjectile; i++)
            {
                var targetPos = target.transform.position + m_weaponData.ShotProperties[i].OffsetPosition;
                var aimDirection = (targetPos - transform.position).normalized;
                var projectileRot = Quaternion.AngleAxis(Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg, Vector3.forward);
                var projectileGO = m_projectilePooler.GetPooledGameObject();
                var projectile = projectileGO.GetComponent<Projectile>();
                projectile.Spawn(new ProjectileBuilder()
                    .SetOwner(m_owner)
                    .SetDirection(aimDirection)
                    .SetPosition(transform.position)
                    .SetRotation(projectileRot));
            }
        }
    }
}

