using System;
using SGGames.Script.Core;
using SGGames.Script.Data;
using SGGames.Script.Entity;
using SGGames.Scripts.Entity;
using UnityEngine;

namespace SGGames.Script.Weapons
{
    public class EnemyDefaultRangeWeapon : Weapon, IProjectileSpawner
    {
        [SerializeField] private ObjectPooler m_projectilePooler;
        [SerializeField] private WeaponData m_weaponData;
        private WeaponStateManager m_stateManager;
        private EnemyController m_controller;
        private IWeaponOwner m_owner;
        private ProjectileBuilder m_projectileBuilder;
        private Vector2 m_lastAimDirection;

        private void Update()
        {
            m_stateManager?.Update();
        }

        #region Weapon
        public override void InitializeWeapon(IWeaponOwner owner)
        {
            m_controller = ((EnemyWeaponHandler)owner).GetComponent<EnemyController>();
            m_stateManager = new WeaponStateManager(this, new (Global.WeaponState stateType, IWeaponState state)[]
            {
                (Global.WeaponState.CoolDown, new WeaponCoolDownState())
            });
            
            var coolDownState = m_stateManager.GetState(Global.WeaponState.CoolDown) as WeaponCoolDownState;
            coolDownState.Initialize(m_weaponData);
            
            InitializeProjectileSpawner(new ProjectileBuilder());
        }
        
        public void UpdateAnimationOnAttack()
        {
            
        }

        public override void Attack()
        {
            if (!IsReady) return;
            SpawnProjectile();
            UpdateAnimationOnAttack();
            m_stateManager.SetState(Global.WeaponState.CoolDown);
        }
        
        #endregion
        
        #region Projectile

        public void InitializeProjectileSpawner(ProjectileBuilder builder)
        {
            m_projectileBuilder = builder;
        }
        
        public void SpawnProjectile()
        {
            if (m_controller.CurrentBrain == null) return;
            if (m_controller.CurrentBrain.Target == null) return;
            
            var target = m_controller.CurrentBrain.Target;
            var numberProjectile = m_weaponData.ProjectilePerShot;

            for (int i = 0; i < numberProjectile; i++)
            {
                var targetPos = target.transform.position + m_weaponData.ShotProperties[i].OffsetPosition;
                var aimDirection = (targetPos - transform.position).normalized;
                m_lastAimDirection = aimDirection;
                var projectileRot = Quaternion.AngleAxis(Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg, Vector3.forward);
                var projectileGO = m_projectilePooler.GetPooledGameObject();
                var projectile = projectileGO.GetComponent<Projectile>();
                projectile.Spawn(m_projectileBuilder
                    .SetOwner(m_controller.gameObject)
                    .SetDirection(aimDirection)
                    .SetPosition(transform.position)
                    .SetRotation(projectileRot));
            }
        }
        
        #endregion
        
        public bool IsReady => m_stateManager.IsReady;
    }
}

