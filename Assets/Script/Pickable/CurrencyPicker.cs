using System;
using System.Collections;
using SGGames.Script.Core;
using SGGames.Script.Events;
using UnityEngine;

namespace SGGames.Script.Pickables
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class CurrencyPicker : ItemPicker
    {
        [SerializeField] private Global.ItemID m_currencyID;
        [SerializeField] private int m_amount;
        [SerializeField] private CurrencyEvent m_currencyEvent;
        [SerializeField] private CircleCollider2D m_collider;

        private const float k_FlyingSpeed = 5f;
        private const float k_FlyingSpeedToPlayer = 18f;
        private const float k_OffsetPosition = 1.5F;
        private const float k_DelayBeforeFly = 0.3f;
        
        private Action m_updateMovementAction;
        
        private bool m_shouldFly;
        private Vector2 m_offsetPos;
        private Vector2 m_startPos;
        private float m_lerpT;
        private Transform m_player;
        
        public Global.ItemID ItemID => m_currencyID;
        public int Amount => m_amount;

        private void OnEnable()
        {
            Warmup();
            StartCoroutine(OnDelayBeforeFlyingToPlayer());
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player") && !m_shouldFly) return;

            m_player = other.transform;
            m_collider.enabled = false;
            m_startPos = transform.position;
            m_offsetPos = -(m_player.transform.position - transform.position).normalized * k_OffsetPosition + transform.position;
            m_updateMovementAction = UpdateMovementToOffsetPosition;
            m_shouldFly = true;
        }
        
        private void Update()
        {
            if (!m_shouldFly) return;
            m_updateMovementAction?.Invoke();
        }
        
        private IEnumerator OnDelayBeforeFlyingToPlayer()
        {
            m_collider.enabled = false;
            yield return new WaitForSeconds(k_DelayBeforeFly);
            m_collider.enabled = true;
        }

        /// <summary>
        /// Setup parameters
        /// </summary>
        private void Warmup()
        {
            m_collider.enabled = true;
            m_shouldFly = false;
            m_updateMovementAction = null;
            m_lerpT = 0;
        }

        private void UpdateMovementToOffsetPosition()
        {
            transform.position = Vector2.Lerp(m_startPos,m_offsetPos,m_lerpT);
            m_lerpT += Time.deltaTime * k_FlyingSpeed;
            if (m_lerpT >= 1.0f)
            {
                m_lerpT = 1.0f;
                m_updateMovementAction = UpdateMovementToPlayer;
            }
        }

        private void UpdateMovementToPlayer()
        {
            transform.position = Vector2.MoveTowards(transform.position, m_player.position, k_FlyingSpeedToPlayer * Time.deltaTime);
            if (transform.position == m_player.position)
            {
                m_shouldFly = false;
                m_updateMovementAction = null;
                PickedUp();
                this.gameObject.SetActive(false);
            }
        }

        protected override void OnReceiveGameEvent(Global.GameEventType eventType)
        {
            if (eventType == Global.GameEventType.RoomCreated)
            {
                if (this.gameObject.activeSelf)
                {
                    this.gameObject.SetActive(false);
                }
            }
        }

        protected override void PickedUp()
        {
            m_currencyEvent?.Raise(new CurrencyUpdateData
            {
                ItemID = m_currencyID,
                Amount = m_amount
            });
            base.PickedUp();
        }
    }
}
