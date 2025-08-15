using System;
using SGGames.Script.Data;
using SGGames.Script.HealthSystem;
using UnityEngine;

namespace SGGames.Script.Weapons
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] protected ProjectileData m_projectileData;
        [SerializeField] protected Vector2 m_movingDirection;
        [SerializeField] protected DamageHandler m_damageHandler;
        [SerializeField] protected LayerMask m_obstacleLayerMask;
        [SerializeField] protected BoxCollider2D m_projectileCollider;
        [SerializeField] protected GameObject m_model;
        [SerializeField] protected float m_offsetAngle;
        
        protected GameObject m_owner;
        protected Vector2 m_startPosition;
        protected bool m_isAlive;

        public Action OnProjectileStopped;
        
        public GameObject Owner => m_owner;

        private void Awake()
        {
            m_damageHandler.OnHit += DestroyProjectile;
        }
        
        protected virtual void UpdateMovement()
        {
            transform.Translate(m_movingDirection * (m_projectileData.Speed * Time.deltaTime));    
        }

        protected virtual bool IsOutOfRange()
        {
            return Vector2.Distance(transform.position, m_startPosition) > m_projectileData.Range;
        }

        protected virtual bool IsHitObstacle()
        {
            return Physics2D.OverlapBox(transform.position, m_projectileCollider.size, 0, m_obstacleLayerMask);
        }

        protected virtual void Update()
        {
            if (!m_isAlive) return;
            UpdateMovement();
            if (IsOutOfRange() || IsHitObstacle())
            {
                DestroyProjectile();
            }
        }

        protected virtual void DestroyProjectile()
        {
            OnProjectileStopped?.Invoke();
            m_isAlive = false;
            transform.rotation = Quaternion.identity;
            this.gameObject.SetActive(false);
        }
        
        public virtual void Spawn(ProjectileBuilder builder)
        {
            m_movingDirection = builder.Direction;
            transform.position = builder.Position;
            m_owner = builder.Owner;
            
            m_startPosition = transform.position;
            m_model.transform.rotation = builder.Rotation * Quaternion.AngleAxis(m_offsetAngle, Vector3.forward);
            m_isAlive = true;
        }
    }
}
