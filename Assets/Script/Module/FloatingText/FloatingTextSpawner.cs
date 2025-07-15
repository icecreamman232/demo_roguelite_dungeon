using SGGames.Script.Core;
using SGGames.Script.Data;
using UnityEngine;

namespace SGGames.Script.Modules
{
    public class FloatingTextSpawner : MonoBehaviour
    {
        [SerializeField] private float m_textLifeTime;
        [SerializeField] private ObjectPooler m_textPooler;
        [SerializeField] private FloatingTextEvent m_floatingTextEvent;

        private void Start()
        {
            m_floatingTextEvent.AddListener(OnReceiveSpawnEvent);
        }
        
        private void OnReceiveSpawnEvent(string content, Vector3 spawnPosition)
        {
            var textObject = m_textPooler.GetPooledGameObject();
            var floatingText = textObject.GetComponent<FloatingText>();
            floatingText.SetupFloatingText(content, spawnPosition, m_textLifeTime);
        }

        private void OnDestroy()
        {
            m_floatingTextEvent.RemoveListener(OnReceiveSpawnEvent);
        }
    }
}
