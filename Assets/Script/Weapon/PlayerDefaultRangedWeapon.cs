using SGGames.Script.Core;
using SGGames.Script.Data;
using SGGames.Script.Entity;
using UnityEngine;

namespace SGGames.Script.Weapons
{
    /// <summary>
    /// HybridWeapon is the one that looks like a melee weapon but still spawns the projectiles on attacking.
    /// </summary>
    public class PlayerDefaultRangedWeapon : MonoBehaviour, IWeapon, IProjectileSpawner
    {
        [SerializeField] private WeaponData m_weaponData;
        [SerializeField] private ObjectPooler m_projectilePooler;
        [SerializeField] private Transform m_shootingPivot;
        private WeaponStateManager m_stateManager;
        //private DefaultPlayerWeaponAnimator m_defaultPlayerWeaponAnimator;
        private PlayerWeaponHandler m_playerWeaponHandler;
        private ProjectileBuilder m_projectileBuilder;
        private IWeaponOwner m_owner;
        private GameObject m_ownerGameObject;
        
        public bool IsReady => m_stateManager.IsReady;
        
        // private void Start()
        // {
        //     var animator = GetComponent<Animator>();
        //     if (animator == null)
        //     {
        //         Debug.LogError("Animator not found");
        //     }
        //     m_defaultPlayerWeaponAnimator = new DefaultPlayerWeaponAnimator();
        //     m_defaultPlayerWeaponAnimator.Initialize(animator);
        // }

        private void Update()
        {
            m_stateManager?.Update();
        }
        
        public void SetAttackOnLeft(bool isAttackOnLeft)
        {
            //m_defaultPlayerWeaponAnimator.SetAttackDirection(isAttackOnLeft);
        }

        public void InitializeWeapon(IWeaponOwner owner)
        {
            m_owner = owner;
            m_playerWeaponHandler = (PlayerWeaponHandler)owner;
            m_ownerGameObject = m_playerWeaponHandler.gameObject;
            m_stateManager = new WeaponStateManager(this,
                new (Global.WeaponState stateType, IWeaponState state)[]
                {
                    (Global.WeaponState.CoolDown, new WeaponCoolDownState())
                });

            var coolDownState =m_stateManager.GetState(Global.WeaponState.CoolDown) as WeaponCoolDownState;
            coolDownState.Initialize(m_weaponData);
            
            InitializeProjectileSpawner(new ProjectileBuilder());;
        }

        public void ChangeState(Global.WeaponState nextState)
        {
            m_stateManager.SetState(nextState);
        }

        public void UpdateAnimationOnAttack()
        {
            //m_defaultPlayerWeaponAnimator.UpdateAnimation();
        }

        public void Attack()
        {
            if (!IsReady) return;
            SpawnProjectile();
            UpdateAnimationOnAttack();
            m_stateManager.SetState(Global.WeaponState.CoolDown);
        }
        
        public void InitializeProjectileSpawner(ProjectileBuilder builder)
        {
            m_projectileBuilder = builder;
        }

        public void SpawnProjectile()
        {
            var direction = ((PlayerWeaponHandler)m_owner).AimDirection;
            var projectileRot = Quaternion.AngleAxis(Mathf.Atan2(direction.y,direction.x) * Mathf.Rad2Deg, Vector3.forward);
            var projectileGO = m_projectilePooler.GetPooledGameObject();
            var projectile = projectileGO.GetComponent<Projectile>();
            projectile.Spawn(m_projectileBuilder
                .SetOwner(m_ownerGameObject)
                .SetDirection(m_playerWeaponHandler.AimDirection)
                .SetPosition(m_shootingPivot.position)
                .SetRotation(projectileRot));
        }
    }
}
