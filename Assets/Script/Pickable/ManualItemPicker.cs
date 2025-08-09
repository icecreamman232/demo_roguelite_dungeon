using SGGames.Script.Core;
using SGGames.Script.Data;
using SGGames.Script.Events;
using UnityEngine;

namespace SGGames.Script.Pickables
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class ManualItemPicker : ItemPicker
    {
        [SerializeField] private ItemData m_itemData;
        [SerializeField] private int m_amount;
        [SerializeField] private InventoryEvent m_inventoryEvent;
        [SerializeField] private EquipInventoryItemEvent m_equipInventoryItemEvent;
        [SerializeField] private InteractEvent m_interactEvent;
        [SerializeField] private CircleCollider2D m_collider2D;
        
        private bool m_isInteracting;
        
        public ItemData ItemData => m_itemData;
        public int Amount => m_amount;
        
        private void Awake()
        {
            m_interactEvent.AddListener(OnReceiveInteractionEvent);
        }

        protected override void OnDestroy()
        {
            m_interactEvent.RemoveListener(OnReceiveInteractionEvent);
            base.OnDestroy();
        }


        private void OnEnable()
        {
            WarmUp();
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

        private void WarmUp()
        {
            m_collider2D.enabled = true;
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
        
        protected override void PickedUp()
        {
            base.PickedUp();
            m_inventoryEvent.Raise(Global.InventoryEventType.Add, m_itemData.ItemID, m_amount);
            m_equipInventoryItemEvent.Raise((InventoryItemData)m_itemData);
            m_collider2D.enabled = false;
            this.gameObject.SetActive(false);
        }
    }
}

