using SGGames.Script.Core;
using SGGames.Script.Events;
using UnityEngine;

namespace SGGames.Script.Modules
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class InteractTrigger : MonoBehaviour
    {
        [SerializeField] private InteractEvent m_interactEvent;
        private bool m_isInteracting;
        
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
    }
}
