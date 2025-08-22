using SGGames.Scripts.Core;
using SGGames.Scripts.Events;
using UnityEngine;

namespace SGGames.Scripts.Modules
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class InteractTrigger : MonoBehaviour
    {
        [SerializeField] private Material m_outlineMaterial;
        [SerializeField] private SpriteRenderer m_spriteRenderer;
        [SerializeField] private InteractEvent m_interactEvent;
        private IInteractable m_interactable;
        private bool m_isInteracting;
        private Material m_defaultMaterial;

        private void Start()
        {
            m_interactEvent.AddListener(OnReceiveEvent);
            m_interactable = GetComponent<IInteractable>();
            m_defaultMaterial = m_spriteRenderer.material;
        }

        private void OnDestroy()
        {
            m_interactEvent.RemoveListener(OnReceiveEvent);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            if (m_isInteracting) return;
            m_isInteracting = true;
            m_spriteRenderer.material = m_outlineMaterial;
            m_interactEvent.Raise(new InteractEventData
            {
                InteractEventType = Global.InteractEventType.Interact,
                Layer = gameObject.layer,
                Tag = gameObject.tag
            });
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            if (!m_isInteracting) return;
            m_isInteracting = false;
            m_spriteRenderer.material = m_defaultMaterial;
            m_interactEvent.Raise(new InteractEventData
            {
                InteractEventType = Global.InteractEventType.Cancel,
                Layer = gameObject.layer,
                Tag = gameObject.tag
            });
        }
        
        private void OnReceiveEvent(InteractEventData interactEventData)
        {
            if (interactEventData.InteractEventType == Global.InteractEventType.Finish
                && m_isInteracting && gameObject.layer == interactEventData.Layer && gameObject.CompareTag(interactEventData.Tag))
            {
                m_interactable.Interact();
            }
        }
    }
}
