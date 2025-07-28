using System.Collections;
using SGGames.Script.Core;
using SGGames.Scripts.Entity;
using UnityEngine;

namespace SGGames.Script.Skills
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class StunAOEIMpact : Impact
    {
        [SerializeField] private CircleCollider2D m_circleCollider;
        [SerializeField] private GameObject m_model;
        [SerializeField] private float m_stunDuration;
        [SerializeField] private float m_stunRadius;

        public override void Initialize(ImpactParamInfo paramInfo)
        {
            var stunAOEParamInfo = paramInfo as StunAOEParamInfo;
            m_stunDuration = stunAOEParamInfo.StunDuration;
            m_stunRadius = stunAOEParamInfo.StunRadius;
            m_circleCollider.enabled = false;
            m_circleCollider.radius = m_stunRadius;
        }

        public override void Execute()
        {
            Debug.Log("Impact::Created Stun AOE");
            StartCoroutine(OnStunning());
        }
        
        protected override void Finished()
        {
            Destroy(this.gameObject);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (LayerManager.IsInLayerMask(other.gameObject.layer, LayerMask.GetMask("Enemy")))
            {
                var enemyController = other.gameObject.GetComponent<EnemyController>();
                if (!enemyController) return;
                enemyController.Movement.ApplyStun(m_stunDuration);
                    
            }
        }

        private IEnumerator OnStunning()
        {
            m_circleCollider.enabled = true;
            m_model.SetActive(true);
            yield return new WaitForSeconds(m_stunDuration);
            m_circleCollider.enabled = false;
            m_model.SetActive(false);
            Finished();
        }
    }
}
