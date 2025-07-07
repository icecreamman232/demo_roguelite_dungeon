using System.Collections;
using UnityEngine;

namespace SGGames.Script.Pickables
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class AutoPickable : Pickable
    {
        [SerializeField] private CircleCollider2D m_collider;
        private static float S_FLYING_SPEED = 5f;
        private static float S_FLYING_SPEED_TO_PLAYER = 18f;
        private static float S_OFFSET_POSITION = 1.5F;
        private static float S_DELAY_BEFORE_FLY = 0.3f;
        
        private delegate void UpdatePickableMovementDelegate();
        private UpdatePickableMovementDelegate m_updateMovementDelegate;
        
        private bool m_shouldFly;
        private Vector2 m_offsetPos;
        private Vector2 m_startPos;
        private float m_lerpT;
        private Transform m_player;

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
            m_offsetPos = -(m_player.transform.position - transform.position).normalized * S_OFFSET_POSITION + transform.position;
            m_updateMovementDelegate = UpdateMovementToOffsetPosition;
            m_shouldFly = true;
        }

        private IEnumerator OnDelayBeforeFlyingToPlayer()
        {
            m_collider.enabled = false;
            yield return new WaitForSeconds(S_DELAY_BEFORE_FLY);
            m_collider.enabled = true;
        }
        
        private void Update()
        {
            if (!m_shouldFly) return;
            m_updateMovementDelegate?.Invoke();
        }

        /// <summary>
        /// Setup parameters
        /// </summary>
        private void Warmup()
        {
            m_collider.enabled = true;
            m_shouldFly = false;
            m_updateMovementDelegate = null;
            m_lerpT = 0;
        }

        private void UpdateMovementToOffsetPosition()
        {
            transform.position = Vector2.Lerp(m_startPos,m_offsetPos,m_lerpT);
            m_lerpT += Time.deltaTime * S_FLYING_SPEED;
            if (m_lerpT >= 1.0f)
            {
                m_lerpT = 1.0f;
                m_updateMovementDelegate = UpdateMovementToPlayer;
            }
        }

        private void UpdateMovementToPlayer()
        {
            transform.position = Vector2.MoveTowards(transform.position, m_player.position, S_FLYING_SPEED_TO_PLAYER * Time.deltaTime);
            if (transform.position == m_player.position)
            {
                m_shouldFly = false;
                m_updateMovementDelegate = null;
                PickedUp();
                this.gameObject.SetActive(false);
            }
        }
    }
}
