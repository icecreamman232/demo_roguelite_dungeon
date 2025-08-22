using SGGames.Scripts.Core;
using SGGames.Scripts.Events;
using UnityEngine;

namespace SGGames.Scripts.Pickables
{
    public class ItemPicker : MonoBehaviour
    {
        [SerializeField] protected Global.PickableType m_pickableType;
        [SerializeField] protected GameEvent m_gameEvent;
        
        public Global.PickableType PickableType => m_pickableType;

        protected virtual void Start()
        {
            m_gameEvent.AddListener(OnReceiveGameEvent); 
        }
        
        protected virtual void OnDestroy()
        {
            m_gameEvent.RemoveListener(OnReceiveGameEvent); 
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
            
        }
    }
}
