using SGGames.Script.Core;
using SGGames.Script.Data;
using SGGames.Script.Entity;
using SGGames.Script.Modules;
using SGGames.Scripts.Entity;
using UnityEngine;

namespace SGGames.Script.Weapons
{
    public class EnemyRangeWeapon : Weapon, IProjectileSpawner, IStateTransitioner<Global.WeaponState>
    {
        [SerializeField] private ObjectPooler m_projectilePooler;
        [SerializeField] private WeaponData m_weaponData;
        private EnemyWeaponStateMachine m_stateMachine;
        private EnemyController m_controller;
        private IWeaponOwner m_owner;
        private ProjectileBuilder m_projectileBuilder;
        
        public int NumberSpawnedProjectile { get; set; }

        public bool IsReady => m_stateMachine.CurrentState is EnemyWeaponReadyState;
        
        private void Update()
        {
            m_stateMachine?.Update();
        }

        #region Weapon
        public override void InitializeWeapon(IWeaponOwner owner)
        {
            m_controller = ((EnemyWeaponHandler)owner).GetComponent<EnemyController>();
            m_stateMachine = new EnemyWeaponStateMachine(this, new (Global.WeaponState stateType, IState<Weapon> state)[]
            {
                (Global.WeaponState.Ready, new EnemyWeaponReadyState()),
                (Global.WeaponState.InProgress, new EnemyWeaponInProgressState()),
                (Global.WeaponState.Complete, new EnemyWeaponCompleteState(this)),
                (Global.WeaponState.CoolDown, new WeaponCoolDownState()),
            });
            
            var coolDownState = m_stateMachine.GetState(Global.WeaponState.CoolDown) as WeaponCoolDownState;
            coolDownState.Initialize(m_weaponData);
            
            InitializeProjectileSpawner(new ProjectileBuilder());
        }

        public override bool IsAttacking()
        {
            return !IsReady;
        }

        public override void Attack()
        {
            if (!IsReady) return;
            SpawnProjectile();
            UpdateAnimationOnAttack();
            m_stateMachine.SetState(Global.WeaponState.InProgress);
        }
        
        #endregion
        
        #region Projectile

        public void InitializeProjectileSpawner(ProjectileBuilder builder)
        {
            m_projectileBuilder = builder;
        }
        
        public void SpawnProjectile()
        {
            if (m_controller.AIBrain == null) return;
            if (m_controller.AIBrain.Target == null) return;
            
            var target = m_controller.AIBrain.Target;
            var numberProjectile = m_weaponData.ProjectilePerShot;

            for (int i = 0; i < numberProjectile; i++)
            {
                var targetPos = target.transform.position + m_weaponData.ShotProperties[i].OffsetPosition;
                var aimDirection = (targetPos - transform.position).normalized;
                var projectileRot = Quaternion.AngleAxis(Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg, Vector3.forward);
                var projectileGO = m_projectilePooler.GetPooledGameObject();
                var projectile = projectileGO.GetComponent<Projectile>();
                projectile.Spawn(m_projectileBuilder
                    .SetOwner(m_controller.gameObject)
                    .SetDirection(aimDirection)
                    .SetPosition(transform.position)
                    .SetRotation(projectileRot));
                projectile.OnProjectileStopped = OnProjectileStopped;
            }
        }

        private void OnProjectileStopped()
        {
            NumberSpawnedProjectile--;
            if (NumberSpawnedProjectile <= 0)
            {
                m_stateMachine.SetState(Global.WeaponState.Complete);
            }
        }

        public void ChangeState(Global.WeaponState newState)
        {
            m_stateMachine.SetState(newState);
        }

        #endregion
    }
}

