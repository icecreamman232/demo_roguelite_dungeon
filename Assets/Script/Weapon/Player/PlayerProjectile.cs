using System.Collections;
using UnityEngine;

namespace SGGames.Scripts.Weapons
{
    public class PlayerProjectile : Projectile
    {
        [SerializeField] private Animator m_animator;
        
        private readonly float m_impactAnimDuration = 0.5f;
        private readonly int k_ANIM_IMPACT_PARAM = Animator.StringToHash("Trigger_Impact");

        private void OnEnable()
        {
            m_projectileCollider.enabled = true;
        }
        
        protected override void DestroyProjectile()
        {
            StartCoroutine(DestroyProjectileCoroutine());
        }

        private IEnumerator DestroyProjectileCoroutine()
        {
            m_projectileCollider.enabled = false;
            m_isAlive = false;
            transform.rotation = Quaternion.identity;
            if (m_animator != null)
            {
                m_animator.SetTrigger(k_ANIM_IMPACT_PARAM);
                yield return new WaitForSeconds(m_impactAnimDuration);
            }
            OnProjectileStopped?.Invoke();
            this.gameObject.SetActive(false);
        }
    }
}
