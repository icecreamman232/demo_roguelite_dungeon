using SGGames.Script.Core;
using SGGames.Script.Events;
using UnityEngine;

namespace SGGames.Script.Dungeon
{
    public class Door : MonoBehaviour
    {
        [SerializeField] private Sprite m_openSprite;
        [SerializeField] private Sprite m_closeSprite;
        [SerializeField] private BoxCollider2D m_exitCollider;
        [SerializeField] private BoxCollider2D m_obstacleCollider;
        [SerializeField] private GameEvent m_gameEvent;
        
        private SpriteRenderer m_renderer;
        private bool m_isOpen;
        private void Awake()
        {
            m_gameEvent.AddListener(OnReceiveGameEvent);
            m_renderer = GetComponentInChildren<SpriteRenderer>();
            CloseDoor();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!m_isOpen) return;
            if (!other.CompareTag("Player")) return;
            
            m_gameEvent.Raise(Global.GameEventType.LoadNextRoom);
        }

        private void OpenDoor()
        {
            m_isOpen = true;
            m_renderer.sprite = m_openSprite;
            m_exitCollider.enabled = true;
            m_obstacleCollider.enabled = false;
        }

        private void CloseDoor()
        {
            m_isOpen = false;
            m_renderer.sprite = m_closeSprite;
            m_exitCollider.enabled = false;
            m_obstacleCollider.enabled = true;
        }

        private void OnReceiveGameEvent(Global.GameEventType eventType)
        {
            if (eventType == Global.GameEventType.RoomCleared)
            {
                OpenDoor();
                m_gameEvent.RemoveListener(OnReceiveGameEvent);
            }
        }
    }
}
