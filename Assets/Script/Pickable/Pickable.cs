using SGGames.Script.Core;
using SGGames.Script.Data;
using SGGames.Script.Events;
using UnityEngine;

namespace SGGames.Script.Pickables
{
    public class Pickable : MonoBehaviour
    {
        [SerializeField] protected Global.PickableType m_pickableType;
        [SerializeField] protected ItemData m_itemData;
        [SerializeField] protected int m_amount;
        [SerializeField] protected GameEvent m_gameEvent;
        
        public Global.PickableType PickableType => m_pickableType;
        public ItemData ItemData => m_itemData;
        public int Amount => m_amount;

        protected virtual void Start()
        {
            m_gameEvent.AddListener(OnReceiveGameEvent); 
        }

        protected virtual void OnReceiveGameEvent(Global.GameEventType eventType)
        {
            if (eventType == Global.GameEventType.RoomCreated)
            {
                if (this.gameObject.activeSelf)
                {
                    this.gameObject.SetActive(false);
                }
            }
        }

        /// <summary>
        /// Send item pick data to system that listens to it
        /// </summary>
        protected virtual void PickedUp()
        {
            m_itemData.Picked(m_amount);
        }

        protected virtual void OnDestroy()
        {
            m_gameEvent.RemoveListener(OnReceiveGameEvent); 
        }
    }
}
