using SGGames.Script.Core;
using SGGames.Script.Events;
using UnityEngine;

namespace SGGames.Script.Modules
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class InteractTrigger : MonoBehaviour
    {
        [SerializeField] private InteractEvent m_interactEvent;
        private IInteractable m_interactable;
        private bool m_isInteracting;

        private void Start()
        {
            m_interactEvent.AddListener(OnReceiveEvent);
            m_interactable = GetComponent<IInteractable>();
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            if (m_isInteracting) return;
            m_isInteracting = true;
            m_interactEvent.Raise(Global.InteractEventType.Interact, gameObject.layer, gameObject.tag);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            if (!m_isInteracting) return;
            m_isInteracting = false;
            m_interactEvent.Raise(Global.InteractEventType.Cancel, gameObject.layer, gameObject.tag);
        }
        
        private void OnReceiveEvent(Global.InteractEventType eventType, int interactLayer, string interactTag)
        {
            if (eventType == Global.InteractEventType.Finish
                && m_isInteracting && gameObject.layer == interactLayer && gameObject.CompareTag(interactTag))
            {
                m_interactable.Interact();
            }
        }
    }
}
