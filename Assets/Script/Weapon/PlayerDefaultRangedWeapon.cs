using SGGames.Script.Weapons;
using SGGames.Scripts.Core;
using SGGames.Scripts.Data;
using SGGames.Scripts.Entities;
using SGGames.Scripts.Items;
using SGGames.Scripts.Modules;
using UnityEngine;

namespace SGGames.Scripts.Weapons
{
    public class PlayerDefaultRangedWeapon : Weapon, IProjectileSpawner
    {
        [SerializeField] private WorldEvent m_worldEvent;
        [SerializeField] private WeaponData m_weaponData;
        [SerializeField] private ObjectPooler m_projectilePooler;
        [SerializeField] private ObjectPooler m_specialProjectilePooler;
        [SerializeField] private Transform m_shootingPivot;
        private PlayerWeaponStateMachine m_stateManager;
        //private DefaultPlayerWeaponAnimator m_defaultPlayerWeaponAnimator;
        private PlayerWeaponHandler m_playerWeaponHandler;
        private ProjectileBuilder m_projectileBuilder;
        private ProjectileBuilder m_specialProjectileBuilder;
        private IWeaponOwner m_owner;
        private GameObject m_ownerGameObject;

        private bool m_isInComboWindow;
        private int m_currentComboCount = 0;
        private float m_comboWindowTimer = 0;
        [SerializeField] private float m_comboWindowDuration = 0.5f;
        
        private const int k_MaxComboCount = 3;
        
        public bool IsReady => m_stateManager.CurrentState is WeaponReadyState;
        
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
            UpdateComboWindow();
        }

        private void UpdateComboWindow()
        {
            if (!m_isInComboWindow) return;
            
            m_comboWindowTimer -= Time.deltaTime;
            if (m_comboWindowTimer <= 0)
            {
                ResetCombo();
            }
        }

        public void SetAttackOnLeft(bool isAttackOnLeft)
        {
            //m_defaultPlayerWeaponAnimator.SetAttackDirection(isAttackOnLeft);
        }

        public override void InitializeWeapon(IWeaponOwner owner)
        {
            m_owner = owner;
            m_playerWeaponHandler = (PlayerWeaponHandler)owner;
            m_ownerGameObject = m_playerWeaponHandler.gameObject;
            m_stateManager = new PlayerWeaponStateMachine(this,
                new (Global.WeaponState stateType, IState<Weapon> state)[]
                {
                    (Global.WeaponState.CoolDown, new WeaponCoolDownState()),
                    (Global.WeaponState.AttackCombo, new WeaponAttackComboState())
                });

            var coolDownState =m_stateManager.GetState(Global.WeaponState.CoolDown) as WeaponCoolDownState;
            coolDownState.Initialize(m_weaponData);
            
            var attackComboState =m_stateManager.GetState(Global.WeaponState.AttackCombo) as WeaponAttackComboState;
            attackComboState.Initialize(m_weaponData);
            
            InitializeProjectileSpawner(new ProjectileBuilder());
            m_specialProjectileBuilder = new ProjectileBuilder();
        }
        
        public void UpdateAnimationOnAttack()
        {
            //m_defaultPlayerWeaponAnimator.UpdateAnimation();
        }

        public override void Attack()
        {
            if (!IsReady)
            {
                return;
            }

            bool isComboAttack = m_isInComboWindow && m_currentComboCount > 0;

            if (isComboAttack)
            {
                if (m_currentComboCount == k_MaxComboCount - 1)
                {
                    SpawnSpecialProjectile();
                    UpdateAnimationOnAttack();
                    ResetCombo();
                }
                else
                {
                    SpawnProjectile();
                    StartComboWindow();
                    UpdateAnimationOnAttack();
                    m_currentComboCount++;
                }
            }
            else
            {
                SpawnProjectile();
                StartComboWindow();
                UpdateAnimationOnAttack();
                m_currentComboCount = 1;
                m_stateManager.SetState(Global.WeaponState.AttackCombo);
            }
        }

        private void StartComboWindow()
        {
            m_isInComboWindow = true;
            m_comboWindowTimer = m_comboWindowDuration;
        }

        public void ForceResetCombo()
        {
            ResetCombo();
            m_worldEvent.Raise(Global.WorldEventType.OnComboInterrupted,((PlayerWeaponHandler)m_owner).gameObject,null);
        }
        
        private void ResetCombo()
        {
            m_currentComboCount = 0;
            m_isInComboWindow = false;
            m_comboWindowTimer = 0;
            m_stateManager.SetState(Global.WeaponState.CoolDown);
        }
        
        public void InitializeProjectileSpawner(ProjectileBuilder builder)
        {
            m_projectileBuilder = builder;
        }

        private void SpawnSpecialProjectile()
        {
            var direction = ((PlayerWeaponHandler)m_owner).AimDirection;
            var projectileRot = Quaternion.AngleAxis(Mathf.Atan2(direction.y,direction.x) * Mathf.Rad2Deg, Vector3.forward);
            var projectileGO = m_specialProjectilePooler.GetPooledGameObject();
            var projectile = projectileGO.GetComponent<Projectile>();
            projectile.Spawn(m_specialProjectileBuilder
                .SetOwner(m_ownerGameObject)
                .SetDirection(m_playerWeaponHandler.AimDirection)
                .SetPosition(m_shootingPivot.position)
                .SetRotation(projectileRot));
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
