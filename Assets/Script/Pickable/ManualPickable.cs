using SGGames.Script.Core;
using SGGames.Script.Events;
using UnityEngine;

namespace SGGames.Script.Pickables
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class ManualPickable : Pickable
    {
        [SerializeField] private InteractEvent m_interactEvent;
        [SerializeField] private CircleCollider2D m_collider2D;
        
        private bool m_isInteracting;

        private void Awake()
        {
            m_interactEvent.AddListener(OnReceiveInteractionEvent);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if(!other.CompareTag("Player")) return;

            StartInteraction();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if(!other.CompareTag("Player")) return;

            CancelInteraction();
        }

        protected override void PickedUp()
        {
            base.PickedUp();
            m_collider2D.enabled = false;
            this.gameObject.SetActive(false);
        }

        private void StartInteraction()
        {
            m_interactEvent.Raise(Global.InteractEventType.Interact, gameObject.layer, gameObject.tag);
            m_isInteracting = true;
        }

        private void CancelInteraction()
        {
            m_interactEvent.Raise(Global.InteractEventType.Cancel, gameObject.layer, gameObject.tag);
            m_isInteracting = false;
        }
        
        private void OnReceiveInteractionEvent(Global.InteractEventType eventType, int interactLayer, string interactTag)
        {
            if (eventType == Global.InteractEventType.Finish 
                && m_isInteracting
                && gameObject.layer == interactLayer
                && gameObject.CompareTag(interactTag))
            {
                PickedUp();
            }
        }
    }
}

